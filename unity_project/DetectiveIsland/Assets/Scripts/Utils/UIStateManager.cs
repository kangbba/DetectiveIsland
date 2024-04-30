using System;
using System.Collections.Generic;
using Aroka.Anim;
using UnityEngine;

[System.Serializable]
public class UIPlan<T> where T : Enum
{
    public T UIState;
    [SerializeField] private List<GameObject> _uiElements = new List<GameObject>();

    public List<GameObject> UIElements => _uiElements;
}

public class UIStateManager<T> : MonoBehaviour where T : Enum
{
    [SerializeField] private List<UIPlan<T>> _uiPlans = new List<UIPlan<T>>();
    private HashSet<GameObject> _allUIElements = new HashSet<GameObject>();

    private void Awake()
    {
        foreach (var plan in _uiPlans)
        {
            _allUIElements.UnionWith(plan.UIElements);
        }
    }

    private UIPlan<T> GetUIStatePlans(T uiState)
    {
        foreach (var plan in _uiPlans)
        {
            if (EqualityComparer<T>.Default.Equals(plan.UIState, uiState))
            {
                return plan;
            }
        }
        return null;
    }

    public virtual void SetUIState(T uiState, float totalTime)
    {
        var currentPlan = GetUIStatePlans(uiState);
        if (currentPlan != null)
        {
            foreach (var element in _allUIElements)
            {
                CustomSetActive(element, currentPlan.UIElements.Contains(element), totalTime);
            }
        }
        else
        {
            foreach (var element in _allUIElements)
            {
                CustomSetActive(element, false, totalTime);
            }
        }
    }

    private void CustomSetActive(GameObject go, bool isActive, float totalTime)
    {
        var animParent = go.GetComponent<ArokaAnimParent>();
        if (animParent != null)
        {
            animParent.SetOnAllChildren(isActive, totalTime);
        }
        else
        {
            var anim = go.GetComponent<ArokaAnim>();
            if (anim != null)
            {
                anim.SetAnim(isActive, totalTime);
            }
            else
            {
                go.SetActive(isActive);
            }
        }
    }
}
