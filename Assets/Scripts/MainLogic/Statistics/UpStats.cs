using System.Collections;
using System.Collections.Generic;
using Infrastructure.Dependency;
using Infrastructure.Entity;
using UnityEngine;

public class UpStats : MonoBehaviour
{
    private IStatsConfigRepository statsConfigRepository;

    private void Awake() 
    {
        statsConfigRepository = DependenciesContext.Dependencies.Get<IStatsConfigRepository>();
    }

    private void Start() 
    {
        statsConfigRepository.Get();    
    }
}
