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
        public bool attacking = false;
        public bool walkingBackwards = false;
        public float attackTime = 0f;
        public bool firing = false;
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

    public float moveSpeedMultiplier = 1f;

    float originalRot;

    private float velocityDamping = 1;

    private float defaultPlayerMass = 10f;

    private float rotateTo = 0f;
    private bool doRotate = false;

    //player physics properties
    public float movementSpeed = 10f;
    public float maxMoveSpeed = 50f;
    public float jumpStrength = 1f;

    public ak47 weapon;


    private Vector2 mousePos;

    private bool mouseBehindPlayer = false;

    private Vector3 playerScale;

    private class Cache {
        public string previousPlayerAnimation;
    }

    private Cache cache = new Cache();

    public LayerMask groundLayer;

    public Keys keys = new Keys();
    public Player player = new Player();
    public Limbs limb = new Limbs();

    public bool preventFirstMovement;
    public float movementCooldown = 4f;

    private float timer = 0f;

    public bool allowMove = true;

    public bool hasRifle = false;

    private bool previousFiringState = false;

    private bool hasMainItem = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (player.body == null)
        {
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

        handleLogic();

        handlePlayerControls();

        checkWalls();

        handleAnimations();


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

        if (player.mirror) {
            transform.localScale = new Vector3(
                playerScale.x * -1f,
                playerScale.y,
                playerScale.z
            );
        }
        else {
            transform.localScale = playerScale;
        }




        //print(player.onFloor);
    }

    private void LateUpdate() {
        mimicRig();

    }

    void handleLogic() {
        if (player.attacking) {
            player.attackTime += Time.deltaTime;
            if (player.attackTime > .25f) {
                player.attacking = false;
                player.attackTime = 0;
            }
        }

        if(preventFirstMovement) {
            if(timer > movementCooldown) {
                allowMove = true;
            }
        }

        //print("are we walking backwards? " + (player.walkingBackwards ? "yes" : "no"));
        //print("are we looking backwards? " + (player.mirror ? "yes" : "no"));
        //print("is the mouse behind my head? " + (mouseBehindPlayer ? "yes" : "no"));

        if(player.walking) {
            player.mirror = mousePos.x < transform.position.x;
        }

        player.walkingBackwards =
            (player.mirror && keys.d) ||
            (!player.mirror && keys.a);

    }

    void FixedUpdate() {

    }
    
    void checkWalls() {
        float distance = 0.05f;
        Vector2 origin = new Vector2(
            player.box.bounds.center.x,
            player.box.bounds.min.y
        );

        player.onFloor = false;
        
        if(Physics2D.Raycast(
            new Vector2( player.box.bounds.center.x, player.box.bounds.min.y ),
            Vector2.down,
            0.05f,
            groundLayer
        )) { player.onFloor = true; }

        if (Physics2D.Raycast(
            new Vector2(player.box.bounds.min.x, player.box.bounds.min.y),
            Vector2.down,
            0.05f,
            groundLayer
        )) { player.onFloor = true; }

        if (Physics2D.Raycast(
            new Vector2(player.box.bounds.max.x, player.box.bounds.min.y),
            Vector2.down,
            0.05f,
            groundLayer
        )) { player.onFloor = true; }



        player.againstWall = 0;

        if (Physics2D.Raycast(
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

        if (Input.GetMouseButtonDown(0) && hasRifle) {
            player.firing = true;
        }
        if (Input.GetMouseButtonUp(0) && hasRifle) {
            player.firing = false;
        }
    }

    void handlePlayerControls() {
        //ported from javascript version of evil cat game
        if (!allowMove) { return; }
        if (player.onFloor) {
            player.body.linearVelocityX *= (float)Math.Pow(0.1 * moveSpeedMultiplier, Time.deltaTime);
        } else {
           player.body.linearVelocityX *= (float)Math.Pow(0.8 * moveSpeedMultiplier, Time.deltaTime); //can be changed to 1.3 or 1.1 but would be annoying
        }

        player.sneaking = keys.s;

        player.walking = keys.a || keys.d;


        if (!player.sneaking) {
        //left n right shi
        if (keys.d && player.againstWall != 1) {
            if (player.body.linearVelocityX < maxMoveSpeed* moveSpeedMultiplier) {
                if (player.onFloor) {
                    player.body.linearVelocityX += movementSpeed * moveSpeedMultiplier * Time.deltaTime;
                }
                else {
                    player.body.linearVelocityX += movementSpeed * moveSpeedMultiplier * Time.deltaTime;
                }
            }
        }

        if (keys.a && player.againstWall != -1) {
            if (player.body.linearVelocityX > 0-maxMoveSpeed * moveSpeedMultiplier) {
                if (player.onFloor) {
                    player.body.linearVelocityX -= movementSpeed * moveSpeedMultiplier * Time.deltaTime;
                }
                else {
                    player.body.linearVelocityX -= movementSpeed * moveSpeedMultiplier * Time.deltaTime;
                }
            }
        }
        }

        if(Input.GetMouseButton(0)) {
            if(!player.attacking) {
                player.attacking = true;
            }
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

        if(player.firing) {
            if(!previousFiringState) {
                player.animator.CrossFade("riflefire", .1f);
                weapon.isActive = true;

            }
        } else if (previousFiringState) {
            player.animator.CrossFade("idle", .5f);
            weapon.isActive = false;

        }

        previousFiringState = player.firing;
    }

    void setChildLayers(string layerName) {
        LayerMask layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in transform) {
            child.gameObject.layer = layer;
        }
    }

    void handleAnimations() {
        if(allowMove) {
            if (player.onFloor) {
                //if(player.againstWall != 1)
                player.animator.SetBool("running", keys.a || keys.d);
                player.animator.SetBool("falling", false);
                player.animator.speed = 1.3f * moveSpeedMultiplier;
            } else {
                player.animator.SetBool("falling", true);
            }

            player.animator.SetBool("reversed", player.walkingBackwards);

            if (player.attacking) {
                player.animator.SetBool("attacking", true);
            }
            else {
                player.animator.SetBool("attacking", false);
            }

            player.animator.SetBool("sitting", player.sneaking);



            player.animator.SetBool("stopping", (keys.a && player.body.linearVelocityX > 0) || (keys.d && player.body.linearVelocityX < 0));
        } else {
            player.animator.SetBool("running", false);
            player.animator.speed = 1f;
        }


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
        animator.speed = 1.3f * moveSpeedMultiplier;
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

    public void amputate(string name) {
        name = "head";
        print("og no!");
        //Transform obj = transform.Find("rig").transform.Find("body-rig").transform.Find(name);
        Transform limbBone = transform.Find("rig").transform.Find("body-rig").transform.Find(name);
        Transform limb = transform.Find("rig").transform.Find(name).transform.Find("texture");
        limb.GetComponent<Rigidbody2D>().simulated = true;
        limb.GetComponent<CircleCollider2D>().enabled = true;
        Destroy(limbBone.gameObject);
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
