using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : MonoBehaviour {
    public GameObject towerA, towerB, laser, originA, originB;
    public Color turningOn, turnedOn;
    LineRenderer laserRenderer;
    List<Coord> positionsAffected = new List<Coord>();
    [SerializeField]
    Coord posA, posB;

    bool active = false;
    public bool horizontalTrap = true;

    enum phase {off, starting, started}
    phase currentState;
    int laserTurns = 1, currentTurns = 0;
	// Use this for initialization
	void Start () {
        // laserRenderer = laser.GetComponent<LineRenderer>();
        //RepositionTowers();
      //  Initialize();
      //  RenderLaser(originA.transform.position, originB.transform.position, Color.green);

    }


    public void CreateTrap(Coord a, Coord b)
    {
        posA = a;
        posB = b;
        horizontalTrap = posA.x != posB.x ? true : false;
        
        RepositionTowers();
        Initialize();
        RenderLaser(originA.transform.position, originB.transform.position, Color.green);
    }

    void Initialize()
    {
        if (horizontalTrap)
        {
            int yPos = posA.y;
            int xPos;
            for (int i = posA.x + 1; i < posB.x; i++)
            {
                xPos = i;
                positionsAffected.Add(new Coord(xPos, yPos));
            }
        }else
        {
            int yPos;
            int xPos = posA.x;
            for (int i = posA.y + 1; i < posB.y; i++)
            {
                yPos = i;
                positionsAffected.Add(new Coord(xPos, yPos));
            }
        }
        currentState = phase.off;
    }

    public void RepositionTowers()
    {
       /// posA = a;
       // posB = b;
        towerA.transform.position = BoardManager.Instance.CoordToPosition(posA);
        towerB.transform.position = BoardManager.Instance.CoordToPosition(posB);
        
        //    towerA.transform.rotation = new Quaternion(0, -90, 0, 0);
        if (!horizontalTrap)
        {
            if (posA.y < posB.y)
            {
                towerB.transform.Rotate(new Vector3(0, 90, 0));
                towerA.transform.Rotate(new Vector3(0, -90, 0));
            }else
            {
                towerB.transform.Rotate(new Vector3(0, -90, 0));
                towerA.transform.Rotate(new Vector3(0, 90, 0));
            }
        }
        else
        {
            if (posA.x < posB.x)
                towerB.transform.Rotate(new Vector3(0, 180, 0));
            else
                towerA.transform.Rotate(new Vector3(0, 180, 0));
        }

        BoardManager.Instance.SetInvalidPosition(posA);
        BoardManager.Instance.SetInvalidPosition(posB);
    }

    private void OnEnable()
    {
        laserRenderer = laser.GetComponent<LineRenderer>();
        //RenderLaser(towerA.transform.position, towerB.transform.position, Color.green);
        EventManager.StartListening(Events.EnemiesTurn, DoEffect);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.EnemiesTurn, DoEffect);
    }

    private void DoEffect()
    {
        
        switch (currentState)
        {
            case phase.off:
                laser.SetActive(false);
                currentState = phase.starting;
                break;
            case phase.starting:
                laser.SetActive(true);
                ChangeLaserColor(turningOn);
                currentTurns = 0;
                currentState = phase.started;
                break;
            case phase.started:
                {
                    currentTurns++;
                    ChangeLaserColor(turnedOn);
                    if (currentTurns >= laserTurns)
                        currentState = phase.off;
                    CheckObjectsHit();
                }
                break;
            default:
                break;
        }
        Debug.Log(currentState);

        // currentState += 1;
        //if (currentState > phase.started+1)
        //{
        //    currentState = phase.off;
        //}
        //Debug.Log(currentState);
        //if(currentState == phase.started)
        //{
        //    laser.SetActive(true);
        //    Debug.Log("woieorie");
        //}else
        //{
        //    laser.SetActive(false);
        //}
    }

    void CheckObjectsHit()
    {
        BoardManager.tileType tile;
        for (int i = 0; i < positionsAffected.Count; i++)
        {
            tile = BoardManager.Instance.GetPositionType(positionsAffected[i]);
            if (tile == BoardManager.tileType.enemy)
            {
                BoardManager.Instance.TileAttacked(positionsAffected[i], 200);
            }else if (tile == BoardManager.tileType.player)
            {
                BoardManager.Instance._playerScript.TakeDamage(2000);
            }
        }
    }

    void ChangeLaserColor(Color newColor)
    {
        laserRenderer.startColor = (newColor);
        laserRenderer.endColor = (newColor);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void RenderLaser(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
       // GameObject myLine = new GameObject();
       // myLine.transform.position = start;
       // myLine.AddComponent<LineRenderer>();
     //   LineRenderer lr = laser.GetComponent<LineRenderer>();
    //    laserRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        //laserRenderer.SetColors(color, color);
        ChangeLaserColor(color);
        //laserRenderer.SetWidth(0.1f, 0.1f);
        laserRenderer.startWidth = 0.1f;
        laserRenderer.endWidth = 0.1f;
        laserRenderer.SetPosition(0, start);
        laserRenderer.SetPosition(1, end);

        //laserRenderer.enabled = false;

        laser.SetActive(false);
        // GameObject.Destroy(myLine, duration);
    }
}
