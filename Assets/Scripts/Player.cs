using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] public float movementSpeed;
    [SerializeField] public float health;
    [SerializeField] public int size;
    [SerializeField] public int currentXPToLevel;
    [SerializeField] public int currentXP;
    [SerializeField] public int currentSP;
    // private dynamic buyMenu;
    [SerializeField] public GameObject buyMenu;
    private bool buyMenuLock;
    private Stats stats;
    private float t = 0;
    private int growth = 0;
    private GameObjectAdapterComponent adapterComponent;

    // Start is called before the first frame update
    void Start()
    {
        var name = this.GetInstanceID().ToString();
        camera.GetComponent<CameraFollow>().playerId = name;
        this.name = name;
        // var p = new GameObjectAdapterComponent(this.name, this.GetType());
        // var comp = new DecoratorFactory(p, this).generate(10);
        // stats = comp.extend();
        // health = stats.health;
        this.buyMenuLock = false;

        this.name = this.GetInstanceID().ToString();
        this.adapterComponent = new GameObjectAdapterComponent(this.name, this.GetType());
    }

    // Update is called once per frame
    void Update()
    {
        // GameObject.Find("HEALTH").GetComponent<UnityEngine.UI.Text>().text = ((int)health).ToString();
        HandlePlayerMovement();
        // HandleStayInsideScreen();

        if (camera.orthographicSize < size * 4)
        {
            IncreaseCameraSize(camera.orthographicSize);
        }

        HandleBuyMenu();
    }

    void HandlePlayerMovement()
    {
        Vector3 pos = transform.position;
        if (Input.GetKey("w"))
        {
            pos.y += movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.y -= movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += movementSpeed * Time.deltaTime;
        }

        transform.position = pos;
    }

    System.Collections.IEnumerator SetBuyMenuLock()
    {
        yield return new WaitForSeconds(0.1f);
        
        this.buyMenuLock = false;
    }

    public void Item0()
    {
        this.AttachDecorator("MouthDecorator");

    }
    public void Item1()
    {
        this.AttachDecorator("BasicFinsDecorator");
    }
    public void Item2()
    {
        this.AttachDecorator("EyesDecorator");
    }
    public void Item3()
    {
        this.AttachDecorator("WhiskersDecorator");
    }
    public void Item4()
    {
        this.AttachDecorator("SlimeDecorator");
    }
    public void Item5()
    {
        this.AttachDecorator("SpearDecorator");
    }
    public void Item6()
    {
        this.AttachDecorator("SpikesDecorator");
    }
    public void Item7()
    {
        this.AttachDecorator("ThirdEyeDecorator");
    }
    public void Item8()
    {
        this.AttachDecorator("FishTailDecorator");
    }

    void AttachDecorator(string name)
    {
        var comp = new PlayerDecoratorFactory(this.adapterComponent, this).generate(name);
        this.stats = comp.extend();
        this.health = this.stats.health;
    }

    void HandleBuyMenu()
    {
        if (!this.buyMenuLock && Input.GetKey("p"))
        {
            if (this.buyMenu.activeSelf == false)
            {
                this.buyMenu.SetActive(true);
                this.buyMenuLock = true;
                StartCoroutine(SetBuyMenuLock());
            }
            else
            {
                this.buyMenu.SetActive(false);
                this.buyMenuLock = true;
                StartCoroutine(SetBuyMenuLock());
            }
        }
    }
    
    void HandleStayInsideScreen()
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
        
        var cameraRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);
        
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, cameraRect .xMin + 3, cameraRect .xMax - 3),
            Mathf.Clamp(transform.position.y, cameraRect .yMin + 3, cameraRect .yMax - 3),
            transform.position.z);
    }
    
    private void IncreaseCameraSize(float camSize)
    {
        t += Time.deltaTime;
        camera.orthographicSize = camSize * 5;
    }

    private void IncreaseXPToleven()
    {
        Debug.Log("WTF");
        currentXPToLevel *= (int) 1.2;
    }

    public void IncreaseCurrentXP(int xpGains)
    {
        currentXP += xpGains;
        while (currentXP >= currentXPToLevel)
        {
            IncreaseXPToleven();
            IncreaseSize();
            currentXP = currentXP - currentXPToLevel;
            IncreaseSP();
        }
    }

    private void IncreaseSP()
    {
        currentSP += 1;
    }

    public void DecreaseSP(int amount)
    {
        currentSP -= amount;
        if (currentSP > 0)
        {
            currentSP = 0;
        }
    }

    private void IncreaseSize()
    {
        int amount = 3;
        Vector3 local = transform.localScale;
        transform.localScale = new Vector3(local.x + 0.2f * amount,local.y + 0.2f * amount,local.z + 0.2f * amount);
        size++;
        growth++;
    }
    
}
