using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using ZFrameWork;
using System;

public class SkillItem
{

    public SkillItem(Skill _sk)
    {
        m_sk = _sk;
        guid = Guid.NewGuid().ToString();
        m_nLv = 1;
        m_bCD = false;
        m_fCDTimer = _sk.m_nCD;
    }

    public Skill m_sk;
    public string guid;
    public int m_nLv;
    public bool m_bCD;
    public float m_fCDTimer;
}

[XmlRootAttribute("Root")]
public class Skills
{
    public Skills() { }
    [XmlElementAttribute("Skill")]
    public List<Skill> pSkill = new List<Skill>();
}
[XmlRootAttribute("Skill")]
public class Skill
{
    public Skill() { }

    [XmlAttribute("id")]
    public int m_nID { get; set; }

    [XmlAttribute("name")]
    public string m_strName { get; set; }

    [XmlAttribute("icon")]
    public string m_strIcon { get; set; }

    [XmlAttribute("pos")]
    public int m_nPos { get; set; }

    [XmlAttribute("damage")]
    public int m_nDamage { get; set; }

    [XmlAttribute("coldTime")]
    public int m_nCD { get; set; }

    [XmlAttribute("dis")]
    public int m_nDis { get; set; }
}
