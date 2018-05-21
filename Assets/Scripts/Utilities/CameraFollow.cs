using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    int index = 0, level;
    Vector3 offset, defaultOffset, randomLevelOffset;

    public List<Vector3> offsets;
    public List<int> levelsWithCustomOffset;

    void Start()
    {
        defaultOffset = new Vector3(1, 8, 1);
        //target = GameObject.FindGameObjectWithTag(Tags.Player);
        target = BoardManager.Instance._player;
        level = GameManager.Instance.currentLevel;
        if (levelsWithCustomOffset.Contains(level))
        {
            offset = offsets[levelsWithCustomOffset.IndexOf(level)];
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

