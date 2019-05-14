using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArmyScript : MonoBehaviour// actually I think this will be a parent class
{

    public bool         m_isLeader;
    public int          m_numFollowers = 0;
    // public int  m_team=0;//0 reserved for neutrals
    // public int  type;// going to want an enum or maybe different classes per army type bows/guns swords seem to want to behave differently probably
    public GameObject   m_prefab;
    public float        m_speed=1;
    private int         m_maxFollowers = 3;
    private GameObject  m_leader=null;
    private Transform   m_destination;
    private Rigidbody   m_rb;
    private Object[]    m_formations;
    private int         m_unitID=0;
    private GameObject  m_formation;
    private int         m_currFormation=0;
    private List<GameObject> m_units;
    // Start is called before the first frame update
    void Start()
    {
        if (m_isLeader) {
            m_units = new List<GameObject>();
            m_formations = Resources.LoadAll("Formations", typeof(GameObject));
            m_formation = Instantiate(m_formations[m_currFormation], this.transform)as GameObject;
            m_formation.transform.parent = this.transform;
            m_formation.name = "Formation";
            SpawnUnits(3);// just to spawn some in
        }
        m_rb = GetComponent<Rigidbody>();
        
       
    }

    // Update is called once per frame
    void Update()
    {
        if (m_leader!=null) {

            this.transform.position = Vector3.MoveTowards(this.transform.position, m_destination.position, m_speed);// really basic movement
        }else {
            if (Input.GetKeyDown(KeyCode.Q)) {
                ChangeFormations(true);
            }
            if (Input.GetKeyDown(KeyCode.E)) {
                ChangeFormations(false);
            }
        }
    }
    void SetLeader(GameObject leader) {
        m_leader = leader;
    }
    void SpawnUnits(int amount) {
        for (int i = 0; i < amount; i++) {
            GameObject unit = Instantiate( m_prefab, this.transform.position - this.transform.forward * (3+i), Quaternion.identity)as GameObject;
            ArmyScript unitScript = unit.GetComponent<ArmyScript>();
            unitScript.SetLeader(this.gameObject);
            unitScript.m_unitID = this.m_numFollowers;
            unitScript.m_destination = this.m_formation.transform.GetChild(unitScript.m_unitID).transform;
            this.m_numFollowers++;
            this.m_units.Add(unit);
        }

    }
    void ResetDestination() {
        foreach(GameObject unit in m_units) {
            ArmyScript unitScript = unit.GetComponent<ArmyScript>();
            unitScript.m_destination = this.m_formation.transform.GetChild(unitScript.m_unitID).transform;
        }
    }
    void ChangeFormations(bool forwardChange) {
        Destroy(m_formation);
        m_formation = null;
        int changeBy = 1;
        if (!forwardChange)
            changeBy = -1;
        m_currFormation+= changeBy;
        if (m_currFormation < 0)
            m_currFormation = m_formations.Length-1;
        if (m_currFormation > m_formations.Length - 1) {
            m_currFormation = 0;
        }
        m_formation = Instantiate(m_formations[m_currFormation], this.transform) as GameObject;
        m_formation.transform.parent = this.transform;
        m_formation.name = "Formation";
        ResetDestination();
    }
}
