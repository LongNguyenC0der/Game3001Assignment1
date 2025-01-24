using UnityEngine;

public enum EPlayMode : byte
{
    Standby,
    LinearSeek,
    LinearFlee,
    LinearArrive,
    LinearAvoid
}

public class PlaySceneGameMode : MonoBehaviour
{
    private const float BOUNDARY = 7.0f;
    private const float MOVE_SPEED = 3.0f;
    private const float TURN_SPEED = 100.0f;
    private const float RADIUS = 1.0f;


    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private GameObject enemyPrefab;
    private GameObject player;
    private GameObject target;
    private GameObject enemy;
    
    

    // for arrive algorithm
    
    private float timeToTarget = 2.0f;

    private EPlayMode playMode;

    private void Start()
    {
        player = Instantiate<GameObject>(playerPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        target = Instantiate<GameObject>(targetPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        enemy = Instantiate<GameObject>(enemyPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        player.SetActive(false);
        target.SetActive(false);
        enemy.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
        {
            playMode = EPlayMode.Standby;
            player.SetActive(false);
            target.SetActive(false);
            enemy.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            playMode = EPlayMode.LinearSeek;
            SetUpActors(playMode);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            playMode = EPlayMode.LinearFlee;
            SetUpActors(playMode);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            playMode = EPlayMode.LinearArrive;
            SetUpActors(playMode);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Avoid");
        }

        switch (playMode)
        {
            case EPlayMode.Standby:
                break;
            case EPlayMode.LinearSeek:
                LinearSeek();
                break;
            case EPlayMode.LinearFlee:
                LinearFlee();
                break;
            case EPlayMode.LinearArrive:
                LinearArrive();
                break;
            case EPlayMode.LinearAvoid:
                break;
            default:
                break;
        }
    }

    private void SetUpActors(EPlayMode playMode)
    {
        player.SetActive(false);
        target.SetActive(false);
        enemy.SetActive(false);

        switch (playMode)
        {
            case EPlayMode.Standby:
                break;
            case EPlayMode.LinearSeek:
            case EPlayMode.LinearArrive:
                player.transform.position = GetRandomPosition();
                target.transform.position = GetRandomPosition();
                player.SetActive(true);
                target.SetActive(true);
                break;
            case EPlayMode.LinearFlee:
                player.transform.position = GetRandomPosition();
                enemy.transform.position = GetRandomPosition();
                player.SetActive(true);
                enemy.SetActive(true);
                break;
            case EPlayMode.LinearAvoid:
                break;
            default:
                break;
        }
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-BOUNDARY, BOUNDARY), 0.0f, Random.Range(-BOUNDARY, BOUNDARY));
    }

    private void LinearSeek()
    {
        Vector3 direction = (target.transform.position - player.transform.position).normalized;
        Vector3 distanceToMove = direction * MOVE_SPEED * Time.deltaTime;
        player.transform.position += distanceToMove;
        Turning(direction);
    }

    private void LinearFlee()
    {
        Vector3 direction = (player.transform.position - enemy.transform.position).normalized;
        Vector3 distanceToMove = direction * MOVE_SPEED * Time.deltaTime;
        Vector3 newPosition = player.transform.position + distanceToMove;

        float x = newPosition.x;
        float z = newPosition.z;

        if (player.transform.position.x >= BOUNDARY) x = BOUNDARY;
        else if (player.transform.position.x <= -BOUNDARY) x = -BOUNDARY;
        if (player.transform.position.z >= BOUNDARY) z = BOUNDARY;
        else if (player.transform.position.z <= -BOUNDARY) z = -BOUNDARY;

        newPosition.x = x;
        newPosition.z = z;
        player.transform.position = newPosition;

        Turning(direction);
    }

    private void LinearArrive()
    {
        Vector3 distance = target.transform.position - player.transform.position;
        if (distance.magnitude > RADIUS)
        {
            distance /= timeToTarget;
            if (distance.magnitude > MOVE_SPEED)
            {
                distance = distance.normalized * MOVE_SPEED;
            }
            //Vector3 direction = distance.normalized;
            Vector3 distanceToMove = distance * Time.deltaTime;
            player.transform.position += distanceToMove;
            Turning(distance);
        }
    }

    private void Turning(Vector3 direction)
    {
        float currentAngle = player.transform.eulerAngles.y;
        float desiredAngle = Vector3.SignedAngle(player.transform.forward, direction, Vector3.up); // Return relative angle
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, currentAngle + desiredAngle, TURN_SPEED * Time.deltaTime); // 2 absolute angles
        player.transform.rotation = Quaternion.Euler(0.0f, newAngle, 0.0f);
    }
}
