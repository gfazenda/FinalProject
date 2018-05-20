using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    int index = 0;
    Vector3 offset, defaultOffset;

    public List<Vector3> offsets;
    public List<int> levelsWithCustomOffset;

    void Start()
    {
        defaultOffset = new Vector3(1, 8, 1); 
        //target = GameObject.FindGameObjectWithTag(Tags.Player);
        target = BoardManager.Instance._player;

        if (levelsWithCustomOffset.Contains(GameManager.Instance.currentLevel))
        {
            offset = offsets[levelsWithCustomOffset.IndexOf(GameManager.Instance.currentLevel)];
        }
        else
        {
            offset = defaultOffset;
        }
    }


    void LateUpdate()
    {
        //float currentAngle = transform.eulerAngles.y;
        //float desiredAngle = target.transform.eulerAngles.y;
        //float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);
        //Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = target.transform.position + (/*rotation **/ offset);

        //transform.LookAt(target.transform);
    }
}

