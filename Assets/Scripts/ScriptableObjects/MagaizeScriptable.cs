using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magazine", menuName = "MagazineConfig")]
public class MagaizeScriptable : ScriptableObject
{
    public ModifierScriptable m_AmmoType;
    public int m_MaxRoundsCapacity;    
    [SerializeField]
    private int m_RemainingRounds;

    //Events
    public GameEventArgs m_OnAmmoConsumed;

    public GameEventArgs m_OnAmmoReloaded;
    public GameEventArgs m_OnMagazineEmpty;

    public int RemainingRounds
    {
        get { return m_RemainingRounds; }
    }

    void OnEnable()
    {
        m_RemainingRounds = m_MaxRoundsCapacity;
    }

    public MagaizeScriptable CreateInstance()
    {
        MagaizeScriptable temp = Instantiate(this);
        temp.m_AmmoType = this.m_AmmoType;
        temp.m_MaxRoundsCapacity = this.m_MaxRoundsCapacity;
        temp.m_RemainingRounds = this.m_RemainingRounds;
        temp.m_OnAmmoConsumed = this.m_OnAmmoConsumed;
        temp.m_OnAmmoReloaded = this.m_OnAmmoReloaded;
        temp.m_OnMagazineEmpty = this.m_OnMagazineEmpty;
        return temp;
    }

    public void ConsumeAmmo()
    {

        if (m_RemainingRounds > 0)
        {
            m_RemainingRounds--;
            m_OnAmmoConsumed.Raise(this);
            if (m_RemainingRounds == 0)
            {
                m_OnMagazineEmpty.Raise(this);
            }
        }
    }

    public void ReloadAmmo()
    {
        if (m_RemainingRounds < m_MaxRoundsCapacity)
        {
            m_RemainingRounds = m_MaxRoundsCapacity;
            m_OnAmmoReloaded.Raise(this);
        }
    }
}
