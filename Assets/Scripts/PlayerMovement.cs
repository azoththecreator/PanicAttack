using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Transform tr;
    Rigidbody2D rb;
	public Transform tr2;
	public Rigidbody2D rb2;
    public KeyCode left, right, up, down;

	[SerializeField] int speed;
	[SerializeField] int jump;
	[SerializeField] public bool canMove = true;
	[SerializeField] bool isJumping = false;

	public float dist;
	public DistanceJoint2D dj;
	float maxDist = 6, defaultDist = 3;
	public SpringJoint2D sj;
	PlayerMovement opponent;

    void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

		opponent = tr2.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (canMove)
        {
			// �¿� ������
            if (Input.GetKey(left))
                tr.Translate(-speed * Time.deltaTime, 0, 0);
            if (Input.GetKey(right))
                tr.Translate(speed * Time.deltaTime, 0, 0);

			// ����
			if (Input.GetKeyDown(up))
            {
                if (!isJumping)
                {
                    rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                    isJumping = true;
                }
            }
		}

        // ����
        if (Input.GetKeyDown(down) && opponent.canMove)
        {
            canMove = false;
            //GetComponent<SpriteRenderer>().color = Color.red; // �ӽ�
			rb.constraints = RigidbodyConstraints2D.FreezePosition;
			dj.distance = maxDist;
		}

		if (Input.GetKeyUp(down))
        {
            canMove = true;
            //GetComponent<SpriteRenderer>().color = Color.white; // �ӽ�
			if (dist > 4)
			{
				sj.frequency = (dist - 3) * .5f;
				sj.enabled = true;
			}
			else
			{
				rb.constraints = RigidbodyConstraints2D.None;
				rb.constraints = RigidbodyConstraints2D.FreezeRotation;
				dj.distance = defaultDist;
			}
		}

		// �Ÿ��� �ٺ��� �� ��� �̵��ӵ� ������
		dist = Vector3.Distance(tr.position, tr2.position);
		if (dist > 3 && speed == 4)
			speed = 2;
		else if (dist <= 3 && speed == 2)
			speed = 4;

		// ���� �� �߻�
		if (sj.enabled && (Mathf.Abs(tr.position.x - tr2.position.x) < .5f || Mathf.Abs(tr.position.y - tr2.position.y) < .5f))
		{
			rb.constraints = RigidbodyConstraints2D.None;
			rb.constraints = RigidbodyConstraints2D.FreezeRotation;
			rb2.constraints = RigidbodyConstraints2D.None;
			rb2.constraints = RigidbodyConstraints2D.FreezeRotation;
			sj.enabled = false;
			dj.distance = defaultDist;
		}
    }

    private void FixedUpdate()
    {
		// ���� üũ
		Debug.DrawRay(rb.position, Vector3.down, Color.red);
        RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector3.down, 1, LayerMask.GetMask("Terrain"));
        if (rayHit.collider != null && rb.velocity.y < 0)
            if (isJumping)
                isJumping = false;
	}
}