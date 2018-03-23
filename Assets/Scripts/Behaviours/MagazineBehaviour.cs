using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineBehaviour : MonoBehaviour
{
    public MagaizeScriptable m_MagazineConfig;
    public MagaizeScriptable m_rtMagazine;


    void Awake()
    {
        m_rtMagazine = m_MagazineConfig.CreateInstance();
    }

    void UseRound()
    {
        m_rtMagazine.ConsumeAmmo();
    }

    void ReloadMag()
    {
        m_rtMagazine.ReloadAmmo();
    }
}
