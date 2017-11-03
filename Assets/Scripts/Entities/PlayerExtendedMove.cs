//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerExtendedMove : MonoBehaviour
//{

//    Use this for initialization

//   void Start()
//    {

//    }

//    void FixedUpdate()
//    {
//        if mouse button(left hand side) pressed instantiate a raycast
//        if (Input.GetMouseButtonDown(0) && !UXManager.instance.TouchOverUI(0))
//        {
//            create a ray cast and set it to the mouses cursor position in game
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            RaycastHit hit;
//            if (Physics.Raycast(ray, out hit))
//            {
//                draw invisible ray cast/ vector
//                if (hit.collider.gameObject.tag == Tags.Ground)
//                {
//                    Debug.DrawLine(ray.origin, hit.point);

//                    BoardManager.Instance.InstantiateEffect(Tags.Poison, hit.collider.gameObject.GetComponent<SpecialTile>().GetPosition());
//                }
//                log hit area to the console
//                / Debug.Log(hit.point);

//            }
//        }
//    }
//}
