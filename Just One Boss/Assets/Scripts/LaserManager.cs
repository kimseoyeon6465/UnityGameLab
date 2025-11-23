using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public static LaserManager instance;

    [Header("Prefabs")]
    public GameObject warningLaserPrefab;
    public GameObject laserPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void FireLaser(Vector3 startPos, Vector3 dir, float warningTime = 0.5f) 
    {
        StartCoroutine(WarningRoutine(startPos, dir, warningTime));
    }

    private IEnumerator WarningRoutine(Vector3 startPos, Vector3 dir, float warningTime)
    {
        GameObject warn = Instantiate(warningLaserPrefab, startPos, Quaternion.identity);
        warn.transform.up = dir;

        yield return new WaitForSeconds(warningTime);

        Destroy(warn );

       
    }
}
