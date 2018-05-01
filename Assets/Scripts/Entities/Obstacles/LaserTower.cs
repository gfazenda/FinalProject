using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : MonoBehaviour {
    public GameObject towerA, towerB, laser, originA, originB;
    LineRenderer laserRenderer;
    List<Coord> positionsAffected = new List<Coord>();
    [SerializeField]
    Coord posA, posB;

    bool active = false;

    enum phase {off, starting, started}
    phase currentState;
    int laserTurns = 1, currentTurns = 0;
	// Use this for initialization
	void Start () {
        laserRenderer = laser.GetComponent<LineRenderer>();
        RepositionTowers(new Coord(1, 3), new Coord(4, 3));
        Initialize();
        RenderLaser(originA.transform.position, originB.transform.position, Color.green);
        
    }

    void Initialize()
    {
        int yPos = posA.y;
        int xPos;
        for (int i = posA.x+1; i < posB.x; i++)
        {
            xPos = i;
            positionsAffected.Add(new Coord(xPos, yPos));
        }
        currentState = phase.off;
    }

    public void RepositionTowers(Coord a, Coord b)
    {
        posA = a;
        posB = b;
        towerA.transform.position = BoardManager.Instance.CoordToPosition(a);
        towerB.transform.position = BoardManager.Instance.CoordToPosition(b);
        towerB.transform.rotation = new Quaternion(0, 180, 0, 0);
        BoardManager.Instance.SetInvalidPosition(a);
        BoardManager.Instance.SetInvalidPosition(b);
    }

    private void OnEnable()
    {
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
                laserRenderer.SetColors(Color.red, Color.red);
                currentTurns = 0;
                currentState = phase.started;
                break;
            case phase.started:
                {
                    currentTurns++;
                    laserRenderer.SetColors(Color.green, Color.green);
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

    // Update is called once per frame
    void Update () {
		
	}

    void RenderLaser(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
       // GameObject myLine = new GameObject();
       // myLine.transform.position = start;
       // myLine.AddComponent<LineRenderer>();
     //   LineRenderer lr = laser.GetComponent<LineRenderer>();
        laserRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        laserRenderer.SetColors(color, color);
        laserRenderer.SetWidth(0.3f, 0.3f);

        laserRenderer.SetPosition(0, start);
        laserRenderer.SetPosition(1, end);
        laser.SetActive(false);
        // GameObject.Destroy(myLine, duration);
    }
}
