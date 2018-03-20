using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExtendedMove : MonoBehaviour
{
    List<GameObject> markers = new List<GameObject>();
    List<Coord> path = new List<Coord>();
    public GameObject markerPrefab;
    bool pathConfirmed = false;
    GameObject currentMarker = null;

    Player _pScript;
    Coord currentPos;
    int markerCount = 0, arraySize = 5, currentIndex = 0;

    void Start()
    {
        _pScript = this.GetComponent<Player>();
        InitializeArray();
    }

    void InitializeArray()
    {
        for (int i = 0; i <= arraySize; i++)
        {
            markers.Add(Instantiate(markerPrefab));
            markers[i].SetActive(false);
        }
    }

    public void AddMarker(Coord pos)
    {
        currentMarker = null;
        currentMarker = ObjectPooler.SharedInstance.GetPooledObject(Tags.Marker);
        if (currentMarker != null)
        {
            currentMarker.transform.position = BoardManager.Instance.CoordToPosition(pos);
            currentMarker.GetComponent<Marker>().EnableMarker(Marker.MarkerType.movement, pos);
            currentMarker.SetActive(true);
            markers.Add(currentMarker);
        }
    }

    public void PlayerClicked(bool selectPath)
    {

        UXManager.instance.ShowPathButton(selectPath);
        if (selectPath)
        {
            
            markerCount = 0;
            // currentPos = new Coord(_pScript.position);
            for (int i = _pScript.position.x - 1; i <= _pScript.position.x + 1; i++)
            {
                for (int j = _pScript.position.y - 1; j <= _pScript.position.y + 1; j++)
                {
                    currentPos = new Coord();
                    currentPos.x = i;
                    currentPos.y = j;
                    //   Debug.Log(currentPos.DebugInfo());
                    if (currentPos.CompareTo(_pScript.position) || currentPos.x != _pScript.position.x && _pScript.position.y != currentPos.y)
                        continue;

                    if (BoardManager.Instance.GetPositionType(currentPos) == BoardManager.tileType.ground)
                    {
                        ActivateMarker(currentPos);
                    }
                }
            }
        }
        else
        {
            DisableMarkers();
        }
    }


    void DisableMarkers()
    {
        for (int i = 0; i < markers.Count; i++)
        {
            markers[i].SetActive(false);
        }
    }

    void DisableAllButOne(bool verticalSwipe, bool positive)
    {
        Coord position = new Coord(_pScript.position);
        int value = positive ? 1 : -1;
        if (verticalSwipe)
        {
            position.y += value;
        }else
        {
            position.x += value;
        }
        for (int i = 0; i < markerCount; i++)
        {
            //   Debug.Log(markers[i].transform.position);
           // if (!markers[i].GetComponent<Marker>().position.CompareTo(position))
          //  {
                markers[i].SetActive(false);
           // }    
        }
    }

    void ActivateMarker(Coord mPosition)
    {
        markers[markerCount].transform.position = BoardManager.Instance.CoordToPosition(mPosition);
        markers[markerCount].GetComponent<Marker>().EnableMarker(Marker.MarkerType.information, mPosition);
        markers[markerCount].SetActive(true);
        markerCount++;
    }

    public void CalculateSwipe(bool verticalSwipe, float swipeDist, float minimumSwipe)
    {
        //DisableAllButOne(verticalSwipe, (swipeDist > 0));

        DisableMarkers();
        markerCount = 1;
        //  
        int positions = Mathf.Abs((int)(swipeDist / minimumSwipe));
        positions = positions > 5 ? 5 : positions;
        //if (positions < 2)
        //    return;
        bool initial = true;
        path.Clear();

        if (swipeDist > 0)
        {
            for (int i = 1; i <= positions; i++)
            {
                Coord newpos = new Coord(_pScript.position);

                if (verticalSwipe)
                    newpos.y += i;
                else
                {
                    newpos.x += i;
                }


                if (BoardManager.Instance.GetPositionType(newpos) == BoardManager.tileType.ground)
                {
                    ActivateMarker(newpos);
                    path.Add(newpos);
                }
                else if (initial)
                {
                    if (verticalSwipe)
                        _pScript.TentativeMove(0, i);
                    else
                        _pScript.TentativeMove(i, 0);

                    break;
                }

                initial = false;
                //else
                //{
                //    break;
                //}
                //  Debug.Log(newpos.DebugInfo());

            }
        }
        else
        {
            for (int i = -1; i >= (-positions); i--)
            {
                Coord newpos = new Coord(_pScript.position);

                if (verticalSwipe)
                    newpos.y += i;
                else
                {
                    newpos.x += i;
                }


                if (BoardManager.Instance.GetPositionType(newpos) == BoardManager.tileType.ground)
                {
                    ActivateMarker(newpos);
                    path.Add(newpos);
                }
                //else if (BoardManager.Instance.GetPositionType(newpos) == BoardManager.tileType.enemy)
                //{
                //    path.Add(newpos);
                //}
                else if(initial)
                {
                    if (verticalSwipe)
                        _pScript.TentativeMove(0, i);
                    else
                        _pScript.TentativeMove(i, 0);

                    break;
                }

                initial = false;
            }
        }
    }

    public void ConfirmPath()
    {
        UXManager.instance.ShowPathButton(false);
        if (path.Count == 0)
        {
            CancelMovement();
            return;
        }
        currentIndex = 0;
        pathConfirmed = true;
        
        UXManager.instance.ShowPathButton(false);
        UXManager.instance.ShowCancelPath(true);

        DisableMarkers();
        markerCount = 0;

        for (int i = 0; i < path.Count; i++)
        {
            markers[i].transform.position = BoardManager.Instance.CoordToPosition(path[i]);
            markers[i].GetComponent<Marker>().EnableMarker(Marker.MarkerType.attack, path[i]);
            markers[i].SetActive(true);
        }

        //markerCount = path.Count;
        //path.Reverse();
        //markers.Reverse();
    }

    public bool PathConfigured()
    {
        return pathConfirmed;
    }

    public Coord NextPosition()
    {
        Coord nextPos = new Coord(path[currentIndex]);
        
        markers[currentIndex].SetActive(false);
        currentIndex++;

        if(currentIndex == path.Count)
        {
            pathConfirmed = false;
            CancelMovement();
        }
        return nextPos;
    }

    public void CancelMovement()
    {
        UXManager.instance.ShowCancelPath(false);
        pathConfirmed = false;
        path.Clear();
        markerCount = 0;
        DisableMarkers();
    }


}
