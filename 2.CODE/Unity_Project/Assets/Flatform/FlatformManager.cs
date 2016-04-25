using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FlatformManager : MonoBehaviour {

    public Transform prefab;

    public Vector3 startPosition;
    private Vector3 nextPosition;

    public int numberOfObjects;
    public float recycleOffset;
    private Queue<Transform> objectQueue;

    public Vector3 minSize, maxSize, minGap, maxGap;
	public float minY, maxY;

    public Material[] materials;
    public PhysicMaterial[] physicMaterials;

    public Booster booster;
    public Obstacles Obstacle;
    // Use this for initialization


    void Start()
    {
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        GameEventManager.GamePause += GamePause;

        objectQueue = new Queue<Transform>(numberOfObjects);
        for (int i = 0; i < numberOfObjects; i++)
        {
            objectQueue.Enqueue((Transform)Instantiate(
                prefab, new Vector3(0f, 0f, -100f), Quaternion.identity));
        }
        enabled = false;
        
    }
    private void GamePause()
    {
        Time.timeScale = 0;
    }

    private void GameStart()
    {
        nextPosition = startPosition;
        for (int i = 0; i < numberOfObjects; i++)
        {
            Recycle();
        }
        enabled = true;
    }

    private void GameOver()
    {
        enabled = false;

    }
    // Update is called once per frame
    void Update()
    {
        if (objectQueue.Peek().localPosition.x + recycleOffset < Runner.distanceTraveled)
        {
            Recycle();
        }
    }

    

    private void Recycle()
    {
        Transform o = objectQueue.Dequeue();
        int materialIndex = Random.Range(0, materials.Length);
        //materialIndex is type
        float scaleSize = 1.0f;
        Texture2D tex;

        Vector3 scale = new Vector3(
            Random.Range(minSize.x * scaleSize, maxSize.x * scaleSize),
            Random.Range(minSize.y, maxSize.y),
            Random.Range(minSize.z, maxSize.z));


        Vector3 position = nextPosition;
        position.x += scale.x * 0.5f;
        position.y += scale.y * 0.5f;


        o.GetComponent<Renderer>().material = materials[materialIndex];
        o.GetComponent<Collider>().material = physicMaterials[materialIndex];
        switch (materialIndex)
        {
            case 0: //regular
                scaleSize = 1;
                o.GetComponent<Collider>().name = "Regular";
                //tex = Resources.Load("speedUp") as Texture2D;
                //o.GetComponent<Renderer>().material.mainTexture = tex;
                booster.SpawnIfAvailable(position, 1);
                break;
            case 1: //slow
                scaleSize = 0.5f;
                o.GetComponent<Collider>().name = "Slow";
                tex = Resources.Load("block") as Texture2D;
                o.GetComponent<Renderer>().material.mainTexture = tex;
                Obstacle.SpawnIfAvailable(position);
                break;
            case 2: //fast
                scaleSize = 1.5f;
                o.GetComponent<Collider>().name = "Fast";
                tex = Resources.Load("speedUp") as Texture2D;
                o.GetComponent<Renderer>().material.mainTexture = tex;
                booster.SpawnIfAvailable(position, 2);
                break;
            case 3: //UpSideDown
                scaleSize = 2;
                o.GetComponent<Collider>().name = "UpSideDown";
                tex = Resources.Load("autoJump") as Texture2D;
                o.GetComponent<Renderer>().material.mainTexture = tex;
                
                break;
        }
       


        

 
        o.localScale = scale;
        o.localPosition = position;

        
        
        
        objectQueue.Enqueue(o);

        nextPosition += new Vector3(
			Random.Range(minGap.x, maxGap.x) + scale.x,
			Random.Range(minGap.y, maxGap.y),
			Random.Range(minGap.z, maxGap.z));

		if(nextPosition.y < minY){
			nextPosition.y = minY + maxGap.y;
		}
		else if(nextPosition.y > maxY){
			nextPosition.y = maxY - maxGap.y;
		}
    }
}
