using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static playerController;

//excuse me for my awful coding
//genuinly never used c sharp before
public class playerController : MonoBehaviour
{
    public class Keys {
        public bool w = false;
        public bool a = false;
        public bool s = false;
        public bool d = false;
        public bool space = false;
    }

    public class Player {
        public Rigidbody2D body;
        public BoxCollider2D box;
        public Animator animator;
        public bool dead = false;
        public bool onFloor = false;
        public int againstWall = 0;
        public bool hasJumped = false;
        public bool ragdoll = false;
        public bool sneaking = false;
        public string animation = "";
        public bool mirror = false;
        public bool walking = false;
    }

    public class Limbs {
        public Transform head;
        public Transform body;
        public Transform rightArmA;
        public Transform rightArmB;
        public Transform rightHand;
        public Transform leftArmA;
        public Transform leftArmB;
        public Transform leftHand;
        public Transform rightLegA;
        public Transform rightLegB;
        public Transform rightFootA;
        public Transform rightFootB;
        public Transform leftLegA;
        public Transform leftLegB;
        public Transform leftFootA;
        public Transform leftFootB;
    }

    float originalRot;

    public int sixtynine = 69;

    private float velocityDamping = 1;

    private float defaultPlayerMass = 10f;

    private float rotateTo = 0f;
    private bool doRotate = false;

    //player physics properties
    public float movementSpeed = 10f;
    public float maxMoveSpeed = 50f;
    public float jumpStrength = 1f;

    private Vector2 mousePos;

    private bool mouseBehindPlayer = false;

    public Transform bone;

    private Vector3 playerScale;

    private class Cache {
        public string previousPlayerAnimation;
    }

    private Cache cache = new Cache();

    public LayerMask groundLayer;

    public Keys keys = new Keys();
    public Player player = new Player();
    public Limbs limb = new Limbs();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.body == null)
        {
            print("houston, we have a problem.");
            player.body = GetComponent<Rigidbody2D>();
            player.box = GetComponent<BoxCollider2D>();
            player.animator = transform.Find("rig").GetComponent<Animator>();
            playerScale = gameObject.transform.localScale;
            resetPlayerDoll();
            //a
            setBodyParts();
        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        handleInput();

        handlePlayerMovement();

        checkWalls();

        handleAnimations();

        if(player.mirror) {
            transform.localScale = new Vector3(
                playerScale.x * -1f,
                playerScale.y,
                playerScale.z
            );
        } else {
            transform.localScale = playerScale;
        }

        if(player.animation != cache.previousPlayerAnimation) {
            cache.previousPlayerAnimation = player.animation;
            setAnimation(player.animation);
        }

        if (Input.GetKeyDown("r")) {
            if(player.ragdoll) {
                resetPlayerDoll();
            } else {
                ragdoll();
            }
            player.ragdoll = !player.ragdoll;
        }

        if (Input.GetKeyDown("m")) {
            amputate("RightArmA");
        }
        float angle = transform.eulerAngles.z;

        //rb.linearVelocityX /= 1f / Time.deltaTime;
        if (doRotate) {
            print(angle);
        }

        print(player.animation);

        //print(player.onFloor);
    }

    private void LateUpdate() {
        mimicRig();

    }

    void FixedUpdate() {

    }
    
    void checkWalls() {
        float distance = 0.05f;
        Vector2 origin = new Vector2(
            player.box.bounds.center.x,
            player.box.bounds.min.y
        );

        player.onFloor = Physics2D.Raycast(
            new Vector2( player.box.bounds.center.x, player.box.bounds.min.y ),
            Vector2.down,
            0.05f,
            groundLayer
        );
            
        if(Physics2D.Raycast(
            new Vector2(player.box.bounds.min.x, player.box.bounds.center.y),
            Vector2.left,
            distance,
            groundLayer
        )) {
            player.againstWall = -1;
        }

        if (Physics2D.Raycast(
            new Vector2(player.box.bounds.max.x, player.box.bounds.center.y),
            Vector2.right,
            distance,
            groundLayer
        )) {
            player.againstWall = 1;
        } else {
            player.againstWall = 0;
        }
    }

    void setBodyParts() {
        limb.head = transform.Find("head");
        limb.body = transform.Find("body");

        limb.leftArmA = transform.Find("leftArmA");
        limb.leftArmB = transform.Find("leftArmB");
        limb.leftHand = transform.Find("leftHand");

        limb.rightArmA = transform.Find("rightArmA");
        limb.rightArmB = transform.Find("rightArmB");
        limb.rightHand = transform.Find("rightHand");

        limb.leftLegA = transform.Find("leftLegA");
        limb.leftLegB = transform.Find("leftLegB");
        limb.leftFootA = transform.Find("leftFootA");
        limb.leftFootB = transform.Find("leftFootB");

        limb.rightLegA = transform.Find("rightLegA");
        limb.rightLegB = transform.Find("rightLegB");
        limb.rightFootA = transform.Find("rightFootA");
        limb.rightFootB = transform.Find("rightFootB");

        print(limb.head.transform.position);
    }

    void handleInput() {
        if (Input.GetKeyDown(KeyCode.W)) { keys.w = true; }
        if (Input.GetKeyDown(KeyCode.A)) { keys.a = true; }
        if (Input.GetKeyDown(KeyCode.S)) { keys.s = true; }
        if (Input.GetKeyDown(KeyCode.D)) { keys.d = true; }
        if (Input.GetKeyDown(KeyCode.Space)) { keys.space = true; }

        if (Input.GetKeyUp(KeyCode.W)) { keys.w = false; }
        if (Input.GetKeyUp(KeyCode.A)) { keys.a = false; }
        if (Input.GetKeyUp(KeyCode.S)) { keys.s = false; }
        if (Input.GetKeyUp(KeyCode.D)) { keys.d = false; }
        if (Input.GetKeyUp(KeyCode.Space)) { keys.space = false; }
    }

    void handlePlayerMovement() {
        //ported from javascript version of evil cat game
        if (player.onFloor) {
            player.body.linearVelocityX *= (float)Math.Pow(0.1, Time.deltaTime);
        } else {
           player.body.linearVelocityX *= (float)Math.Pow(0.8, Time.deltaTime); //can be changed to 1.3 or 1.1 but would be annoying
        }

        player.sneaking = keys.s;

        if(!player.sneaking) {
        //left n right shi
        if (keys.d && player.againstWall != 1) {
            if (player.body.linearVelocityX < maxMoveSpeed) {
                if (player.onFloor) {
                    player.body.linearVelocityX += movementSpeed * Time.deltaTime;
                }
                else {
                    player.body.linearVelocityX += movementSpeed * Time.deltaTime;
                }
            }
        }

        if (keys.a && player.againstWall != -1) {
            if (player.body.linearVelocityX > 0-maxMoveSpeed) {
                if (player.onFloor) {
                    player.body.linearVelocityX -= movementSpeed * Time.deltaTime;
                }
                else {
                    player.body.linearVelocityX -= movementSpeed * Time.deltaTime;
                }
            }
        }
        }


        if (mousePos.x < transform.position.x) {
            mouseBehindPlayer = true;
        }
        else if (mousePos.x > transform.position.x) {
            mouseBehindPlayer = false;
        }

        if (keys.a || keys.d) {
            player.walking = true;
            if (player.onFloor) {
                player.mirror = mouseBehindPlayer;
            }
        } else {
            player.walking = false;
        }

        // && player.onFloor
        if(keys.w || keys.space) {
            if (!player.hasJumped && player.onFloor) {
                player.body.linearVelocityY = jumpStrength;
                player.hasJumped = true;
            }
        } else {
            if(player.onFloor) {
                player.hasJumped = false;
            }
        }
    }

    void setChildLayers(string layerName) {
        LayerMask layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in transform) {
            child.gameObject.layer = layer;
        }
    }

    void handleAnimations() {
        print(player.onFloor);
        if (player.onFloor) {
            player.animator.SetBool("running", keys.a || keys.d);
            player.animator.SetBool("falling", false);
        } else {
            player.animator.SetBool("falling", true);
        }

        if((mouseBehindPlayer && !player.mirror) || (!mouseBehindPlayer && player.mirror)) {
            player.animator.SetFloat("speed", -1);
        } else {
            player.animator.SetFloat("speed", 1);
        }

        player.animator.SetBool("sitting", player.sneaking);

        player.animator.SetBool("stopping", (keys.a && player.body.linearVelocityX > 0) || (keys.d && player.body.linearVelocityX < 0));

        //print(player.body.linearVelocityX);
    }

    void setChildRig(bool enabled)
    {
        foreach (Transform child in transform)
        {
            SpriteSkin skin = child.GetComponent<SpriteSkin>();
            if (skin != null) {
                skin.enabled = enabled;
            }
            Rigidbody2D body = child.GetComponent<Rigidbody2D>();
            if (body != null)
            {
                body.simulated = !enabled;
            }
        }
    }

    void setChildVisibility(bool visibility) {
        foreach (Transform child in transform) {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = visibility;
        }
    }

    void storeChildPos() {
        foreach (Transform child in transform) {
            Vector2 pos = gameObject.transform.position;
        }
    }

    void restoreChildPos() {
        foreach (Transform child in transform) {
        }
    }

    void resetChildPos()
    {
        foreach (Transform child in transform)
        {
            child.position = gameObject.transform.position;

            Rigidbody2D childBody = child.GetComponent<Rigidbody2D>();
            if (childBody != null) {
                childBody.linearVelocity = player.body.linearVelocity;
            }
        }
    }

    void mimicRig() {
        return;
        Transform rig = transform.Find("rig");

        Transform rightArmA = transform.Find("rightArmA");

        rightArmA.transform.position = rig.transform.position;

    }

    void setRigVisibility(bool visibility) {

    }
    
    void setAnimation(string animation) {
        Transform rig = transform.Find("rig");

        Animator animator = rig.GetComponent<Animator>();
        //animator.Play(animation);
        animator.speed = 1.3f;
        return;

    }

    void ragdoll() {
        gameObject.layer = LayerMask.NameToLayer("ghost");
        player.body.mass = 0.0001f;
        player.body.freezeRotation = false;
        //setChildVisibility(false);

        setFaceTexture("hurt");

        setChildLayers("player");
        print("initiated ragdoll mode");
    }

    void resetPlayerDoll() {
        gameObject.layer = LayerMask.NameToLayer("player");
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.body.mass = defaultPlayerMass;


        player.body.freezeRotation = true;
        //setChildVisibility(true);

        setFaceTexture("angry");

        setChildLayers("ghost");
        print("walky mode on");

        float angle = transform.eulerAngles.z;
        //doRotate = trues;
    }

    void amputate(string name) {
        print("og no!");
        Transform obj = transform.Find(name);
        Destroy(obj);
    }

    void setFaceTexture(string name) {
        print("setting face texture to " + name);
        return;
        Transform face = transform.Find("head").transform.Find("face");

        SpriteRenderer faceObj = face.gameObject.GetComponent<SpriteRenderer>();

        faceObj.sprite = null;
    }

    //what i wanna do here is basically make the shit point in a certain direction and do that for each limb for every couple of frames to animate walking
    void pointLimb(string part, float target) {
        //Transform bodypart = print(limb[part])

        //HingeJoint2D joint = bodypart.GetComponent<HingeJoint2D>();

        //print(joint);
        //Destroy(joint);
    }
}
