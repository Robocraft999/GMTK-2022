using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceUIManager : MonoBehaviour
{
    public Transform dice;
    public Camera diceCam;
    public float stopSpeed;

    public DiceUIManager Instance { get; private set; }
    private IEnumerator rotator = null;

    private Quaternion targetRotation;
    private Vector3 rotationSpeed;
    public int Result { get; private set; } = -1;

    void Start()
    {
        Instance = this;
    }

    public bool IsRotating()
    {
        return rotator != null;
    }

    public void StartRotate()
    {
        if(rotator == null)
        {
            rotator = Rotator();
            targetRotation = transform.rotation;
            rotationSpeed = new Vector3(Random.Range(10f, 50f), Random.Range(10f, 50f), Random.Range(10f, 50f));
            StartCoroutine(rotator);
        }
    }

    private int CalcResult()
    {
        Ray ray2 = new Ray(diceCam.transform.position, dice.position - diceCam.transform.position);
        if (Physics.Raycast(ray2, out var hit))
        {
            if(int.TryParse(hit.transform.gameObject.name, out var result))
            return result;
        }
        return -1;
    }

    private IEnumerator Rotator()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            
            if (rotationSpeed.sqrMagnitude > 0.05)
            {
                transform.rotation *= Quaternion.Euler(rotationSpeed.x, rotationSpeed.y, rotationSpeed.z);
            }
            else
            {
                targetRotation = Quaternion.Euler(
                    (float)System.Math.Round(transform.rotation.eulerAngles.x / 90) * 90,
                    (float)System.Math.Round(transform.rotation.eulerAngles.y / 90) * 90,
                    (float)System.Math.Round(transform.rotation.eulerAngles.z / 90) * 90
                );
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
            }

            rotationSpeed = Vector3.Lerp(rotationSpeed, Vector3.zero, Time.deltaTime * stopSpeed);
            if(rotationSpeed.magnitude <= 0.03)//targetRotation == transform.rotation || 
            {
                transform.rotation = targetRotation;
                Result = CalcResult();
                StopCoroutine(rotator);
                rotator = null;
            }
        }
    }
}
