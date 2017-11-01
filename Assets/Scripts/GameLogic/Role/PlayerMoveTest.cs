using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnumCameraState
{
    normal, overshoulder
}
public class PlayerMoveTest : MonoBehaviour
{
    /// **************************
    ///	Is Hover UI 
    /// **************************
    public Func<bool> IsOnUICallback;
    bool isOnUI
    {
        get { return IsOnUICallback != null && IsOnUICallback(); }
    }

    Camera _tarCamera;

    public Vector3 m_vSpawnPos = new Vector3();
    public float m_fSpawnRH = 45f;
    public float m_fCamerDis = 50f;
    public float m_fOrgSpeed = 100f;

    /// **************************
    ///	Is Can Controll By Keyboard 
    /// **************************
    public bool IsKeyboardCtrl = true;
    public FocusCtrl TheFocusCtrl;
    private float moveSpeed = 100f;

    private Vector3 m_vTargetPos;
    private bool m_bAutoMove;
    private NavMeshAgent m_nav;
    public Camera tarCamera
    {
        get
        {
            if (_tarCamera == null)
            {
                _tarCamera = Camera.main;
            }
            return _tarCamera;
        }
    }

    private EnumCameraState _cameraState;

    public EnumCameraState CameraState
    {
        get { return _cameraState; }
        set
        {
            _cameraState = value;
            switch (_cameraState)
            {
                case EnumCameraState.normal:
                    TheFocusCtrl.RotV = 45f;
                    TheFocusCtrl.FocusDistance = 50f;
                    break;
                case EnumCameraState.overshoulder:
                    TheFocusCtrl.RotV = -30;
                    TheFocusCtrl.FocusDistance = 3f;
                    break;
            }
        }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = Mathf.Clamp(value, 1f, 10000f); }
    }

    void Awake()
    {
        //TheFocusCtrl
        GameObject objSpawn = GameObject.Find("Spawn");
        if (objSpawn != null)
        {
            m_vSpawnPos = objSpawn.transform.position;
        }
        TheFocusCtrl = new FocusCtrl(gameObject);
        TheFocusCtrl.CurrentPos = m_vSpawnPos;
        TheFocusCtrl.RotH = m_fSpawnRH;
        TheFocusCtrl.FocusDistance = m_fCamerDis;
        MoveSpeed = m_fOrgSpeed;
        CameraState = EnumCameraState.normal;
        TheFocusCtrl.RefreshBiasPos();

        m_bAutoMove = false;
        m_nav = transform.GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if (m_bAutoMove)
        {

        }
        else
        {
            ManualMove();
        }
    }

    private void AutoMove()
    {
        //Vector3 vCurPos = TheFocusCtrl.CurrentPos;
        //float fDis = Vector3.Distance(m_vTargetPos,vCurPos);
        //if (fDis > 3)
        //{
        //    TheFocusCtrl.CurrentPos = 
        //}

        if (m_nav.enabled)
        {
            float f = Vector3.Distance(transform.position, m_vTargetPos);
            if (f > 3)
            {

            }
            else
            {
                m_nav.isStopped = true;
                m_nav.enabled = false;
            }
        }
    }

    private void ManualMove()
    {
        if (CameraState == EnumCameraState.normal)
        {
            FocusDistanceCtrl();
        }

        CameraRotate();

        if (IsKeyboardCtrl)
        {
            CameraMove(GetMoveInput(false));
        }

        OnGround();
    }

    /// **************************
    ///	保证贴地面移动 
    /// **************************
    private void OnGround()
    {
        RaycastHit hit;
        //--检测角色离开地面的高度，获取角色正下方的地面点，每一帧都同步一下这个Y值，让角色一直贴着地面
        if (Physics.Raycast(TheFocusCtrl.CurrentPos, Vector3.down, out hit, 1000f, LayerMask.GetMask("Ground")))
        {
            Vector3 vPos = hit.point;
            TheFocusCtrl.CurrentPos = new Vector3(TheFocusCtrl.CurrentPos.x, vPos.y, TheFocusCtrl.CurrentPos.z);
        }
    }

    public  Vector3 GetMoveInput(bool allowArrow = true)
    {
        float kz = 0;
        if (allowArrow)
        {
            kz += Input.GetKey(KeyCode.UpArrow) ? 1f : 0f * 1f;
            kz += Input.GetKey(KeyCode.DownArrow) ? -1f : 0f * 1f;
        }
        kz += Input.GetKey(KeyCode.W) ? 1f : 0f * 1f;
        kz += Input.GetKey(KeyCode.S) ? -1f : 0f * 1f;
        float kx = 0;
        if (allowArrow)
        {
            kx -= Input.GetKey(KeyCode.LeftArrow) ? -1f : 0f * 1f;
            kx -= Input.GetKey(KeyCode.RightArrow) ? 1f : 0f * 1f;
        }
        kx -= Input.GetKey(KeyCode.A) ? -1f : 0f * 1f;
        kx -= Input.GetKey(KeyCode.D) ? 1f : 0f * 1f;
        return new Vector3(kx, 0, kz);
    }

    void LateUpdate()
    {
        TheFocusCtrl.RefreshBiasPos();
        SetPosAndLookpoint();
    }

    void Update()
    {
        if (IsKeyboardCtrl)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CameraState =
                    CameraState == EnumCameraState.normal ?
                    EnumCameraState.overshoulder : EnumCameraState.normal;
            }
        }
    }

    void SetPosAndLookpoint()
    {
        if (CameraState == EnumCameraState.overshoulder)
        {
            TheFocusCtrl.height = Mathf.Sin(45f / 180 * Mathf.PI) * TheFocusCtrl.FocusDistance;
            TheFocusCtrl.radius = Mathf.Cos(45f / 180 * Mathf.PI) * TheFocusCtrl.FocusDistance;
            float deltaX = TheFocusCtrl.radius * Mathf.Cos((180 - TheFocusCtrl.RotH) / 180 * Mathf.PI);
            float deltaY = TheFocusCtrl.radius * Mathf.Sin((180 - TheFocusCtrl.RotH) / 180 * Mathf.PI);
            TheFocusCtrl.DeltaVec.Set(deltaX, TheFocusCtrl.height, deltaY);
            tarCamera.transform.position = TheFocusCtrl.CurrentPos + TheFocusCtrl.DeltaVec;
            float lookAtHeight = Mathf.Sin(TheFocusCtrl.RotV / 180 * Mathf.PI) * TheFocusCtrl.FocusDistance;
            Vector3 lookAt = TheFocusCtrl.CurrentPos - new Vector3(0, lookAtHeight, 0);
            tarCamera.transform.LookAt(lookAt);
        }
        else if (CameraState == EnumCameraState.normal)
        {
            tarCamera.transform.position = TheFocusCtrl.CurrentPos + TheFocusCtrl.DeltaVec;
            tarCamera.transform.LookAt(TheFocusCtrl.CurrentPos);
        }
    }

    public void FocusDistanceCtrl()
    {
        if (isOnUI) return;
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float scrollDelta = 1 + TheFocusCtrl.FocusDistance * 0.1f;
        float scrollMulti = 0;

        if (scroll > 0) scrollMulti = -1;
        else if (scroll < 0)    scrollMulti = 1;

        if (scrollMulti != 0)   TheFocusCtrl.FocusDistance += scrollDelta * scrollMulti;
    }

    public void CameraMove(Vector3 moveInput)
    {
        float multi = 1;//
        switch (CameraState)
        {
            case EnumCameraState.normal:
                multi = TheFocusCtrl.FocusDistance / 10f * MoveSpeed / 300f;
                break;
            case EnumCameraState.overshoulder:
                multi = TheFocusCtrl.FocusDistance / 10f * MoveSpeed / 300f * 5;
                break;
            default:
                multi = TheFocusCtrl.FocusDistance / 10f * MoveSpeed / 300f;
                break;
        }
        Vector3 deltaMove = GetScaledVector(moveInput,multi);
        TheFocusCtrl.CurrentPos += MoveTowards((TheFocusCtrl.RotH) / 180 * Mathf.PI, deltaMove);
    }

    public  Vector3 GetScaledVector(Vector3 tar, float multi)
    {
        return new Vector3(tar.x * multi, tar.y * multi, tar.z * multi);
    }

    /// <summary>获取朝目标方向前进后退左右平移后, 坐标的改变</summary>
    /// <param name="rotH">水平面旋转方向</param>
    /// <param name="dirX">左右移动距离</param>
    public  Vector3 MoveTowards(float rotH, float dirX, float dirZ)
    {
        float dx = dirX * Mathf.Sin(rotH) + dirZ * Mathf.Cos(rotH);
        float dz = dirX * Mathf.Cos(rotH) - dirZ * Mathf.Sin(rotH);
        return new Vector3(dx, 0, dz);
    }
    public  Vector3 MoveTowards(float rotH, Vector3 dir)
    {
        float dx = dir.x * Mathf.Sin(rotH) + dir.z * Mathf.Cos(rotH);
        float dz = dir.x * Mathf.Cos(rotH) - dir.z * Mathf.Sin(rotH);
        return new Vector3(dx, 0, dz);
    }

    /// **************************
    ///	Rotate 
    /// **************************
    public void CameraRotate()
    {
        if (Input.GetMouseButton(1))
        {
            TheFocusCtrl.RotH += Input.GetAxis("Mouse X") * 2;
            TheFocusCtrl.RotV -= Input.GetAxis("Mouse Y") * 2;
        }
    }

    /// **************************
    ///	Go To Target Position 
    /// **************************
    public void GoTargetPos(Vector3 _vPos)
    {
        m_bAutoMove = true;
        m_vTargetPos = _vPos;

        if (!m_nav.enabled)
        {
            m_nav.enabled = true;
        }
        m_nav.SetDestination(_vPos);
        m_nav.speed = 20;
    }
}

public class FocusCtrl
{
    bool needRefreshBiasPos = false;
    public Vector3 DeltaVec;
    public float radius = 0;
    public float height = 0;
    Animator _rollAnimator;
    Animator rollAnimator
    {
        get
        {
            if (_rollAnimator == null && m_playerObj != null)
                _rollAnimator = m_playerObj.GetComponent<Animator>();
            return _rollAnimator;
        }
    }

    public GameObject m_playerObj;
    bool isWalk;
    public bool IsWalk
    {
        get { return isWalk; }
        set
        {
            isWalk = value;
            if (rollAnimator != null)
            {
                rollAnimator.SetBool("Dash", isWalk);
            }
        }
    }

    private Vector3 focusPos;
    /// <summary>镜头焦点</summary>
    public Vector3 CurrentPos
    {
        get { return focusPos; }
        set
        {
            Vector3 oldFocus = focusPos;
            IsWalk = oldFocus != value;

            focusPos = value;

            if (m_playerObj != null)
            {
                m_playerObj.transform.position = focusPos;
                m_playerObj.transform.eulerAngles = new Vector3(0, rotH + 90f, 0);
            }
        }
    }

    float rotH = 0;
    /// <summary>水平旋转角度 0~360</summary>
    public float RotH
    {
        get { return rotH; }
        set
        {
            rotH = value;
            rotH = rotH % 360f;
            needRefreshBiasPos = true;
        }
    }

    float rotV = 2;
    /// <summary>垂直旋转角度 2~88, 88度时俯视地面</summary>
    public float RotV
    {
        get { return rotV; }
        set
        {
            rotV = value;
            rotV = Mathf.Clamp(rotV, -88f, 88f);
            needRefreshBiasPos = true;
        }
    }

    /// <summary>镜头离焦点距离</summary>

    float minFocusDis = 1f;
    float maxFocusDis = float.MaxValue;

    private float focusDistance = 1f;
    public float FocusDistance
    {
        get { return focusDistance; }
        set
        {
            focusDistance = Mathf.Clamp(value, minFocusDis, maxFocusDis);
            needRefreshBiasPos = true;
        }
    }

    /// <summary>刷新镜头偏移坐标</summary>
    public void RefreshBiasPos(bool immediatly = false)
    {
        if (needRefreshBiasPos || immediatly)
        {
            height = Mathf.Sin(RotV / 180 * Mathf.PI) * FocusDistance;

            radius = Mathf.Cos(RotV / 180 * Mathf.PI) * FocusDistance;
            float deltaX = radius * Mathf.Cos((180 - RotH) / 180 * Mathf.PI);
            float deltaY = radius * Mathf.Sin((180 - RotH) / 180 * Mathf.PI);
            DeltaVec.Set(deltaX, height, deltaY);
            needRefreshBiasPos = false;
        }
    }

    public  FocusCtrl(GameObject _obj)
    {
        m_playerObj = _obj;
    }
}
