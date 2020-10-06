using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
   

    enum State { Alive, Dying, Transcending };
    enum DebugMode {on, off};
    State state = State.Alive; 
    DebugMode deBugMode = DebugMode.off;

    // Values for thrust and rotational speed.
    [SerializeField] float rcsThrust = 250f;
    [SerializeField] float rocketThrust = 3000f;
     
    // Game sounds
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip levelUp;

    // Particle effects

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem finishParticles;

    // Start is called before the first frame update
    void Start()
    {
      
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();

            RespondToRotateInput();

        }

        resetPosition();

        toggleDebug();
        debugLevelUp();

    }

    private void resetPosition()
    {
        if (Input.GetKey(KeyCode.R))
        {
            rigidBody.transform.position = new Vector3(-45, 5, 0);
            rigidBody.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void toggleDebug()
    {

        if (Input.GetKey(KeyCode.D)) {

            if (deBugMode == DebugMode.on)
            {
                deBugMode = DebugMode.off;
            }
            else
            {

                deBugMode = DebugMode.on;

            }

        }
    }

    private void debugLevelUp()
    {
        if (Input.GetKey(KeyCode.L) && deBugMode == DebugMode.on)
        {
            LoadNextLevel();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || deBugMode == DebugMode.on) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":

                break;

            case "Finish":

                LevelComplete();
                break;

            default:

                PlayerCrashed();

                break;
        }

    }

    private void LevelComplete()
    {
        state = State.Transcending;
        Invoke("LoadNextLevel", 1f); //parameterise time
        LevelUpEffects();
    }

    private void LevelUpEffects()
    {
        finishParticles.Play();
        audioSource.PlayOneShot(levelUp);
    }

    private void PlayerCrashed()
    {
        crashEffects();
        Invoke("LoadFirstLevel", 2f);
    }

    private void crashEffects()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        crashParticles.Play();
        state = State.Dying;
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);

    }

    private void LoadNextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int nextLevel = currentLevel + 1;

        if (nextLevel == SceneManager.sceneCountInBuildSettings)
        {
            nextLevel = 0;
        }
        SceneManager.LoadScene(nextLevel);
         

    }

    private void RespondToRotateInput()
    {

        rigidBody.freezeRotation = true;


        float rotationThisFrame;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotationThisFrame = RotateLeft();

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rotationThisFrame = RotateRight();
        }

        rigidBody.freezeRotation = false;
    }

    private float RotateRight()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        transform.Rotate(Vector3.back * rotationThisFrame);
        return rotationThisFrame;
    }

    private float RotateLeft()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotationThisFrame);
        return rotationThisFrame;
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();

        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * rocketThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
}