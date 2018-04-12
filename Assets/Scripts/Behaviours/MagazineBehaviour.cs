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

    public void UseRound()
    {        
        m_rtMagazine.ConsumeAmmo(this.gameObject);
    }

    public void ReloadMag()
    {
        m_rtMagazine.ReloadAmmo(this.gameObject);
    }
}
