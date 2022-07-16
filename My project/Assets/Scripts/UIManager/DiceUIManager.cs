using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceUIManager : MonoBehaviour
{
    public Transform dice;
    private Vector3 rotationSpeed;
    public float stopSpeed;
    private Quaternion targetRotation;

    public DiceUIManager Instance { get; private set; }
    private IEnumerator rotator = null;

    void Start()
    {
        Instance = this;

        //StartRotate();
    }

    private bool MouseDown()
    {
        return Input.GetMouseButton(0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartRotate();
        }
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

    private IEnumerator Rotator()
    {
        //yield return new WaitUntil(MouseDown);
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
            //if(rotationSpeed == Vector3.zero)
            if(transform.rotation == targetRotation || rotationSpeed == Vector3.zero)
            {
                transform.rotation = targetRotation;
                StopCoroutine(rotator);
                rotator = null;
            }
        }
    }
}
