using UnityEngine;

/// <summary>
/// 1. 플레이어 이동
/// 2. 플레이어 화면 밖으로 못나가게 막기.
/// </summary>


//Y축하면 위로 올라가게끔.
public class PlayerController : MonoBehaviour
{
    //플레이어 이동 구현하기
    // 1.Transfrom.position
    // 2.Transfrom.Translate()
    // 3.Vector3.MoveTowards()
    // 4.마우스 클릭 이동
    // 5.Rigidbody를 사용해서 이동.

    public float speed =5;

    private Vector3 targetPosition; //목표 위치


    private Camera mainCamera;      //메인 카메라.

    public float paddingX = 0.02825f;  //UV 좌표는 0~1사이, 작은 값으로 수치 조정해야 한다.
    public float paddingY = 0.05f;

    //UI 요소 중 마진과 패딩이 있다.
    //마진은 가장 바깥, 그 안쪽이 패딩, 중앙에 콘텐츠가 있다.


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //초기 목표 위치를 현재 위치로 설정
        targetPosition = transform.position;

        //메인카메라 참조 가져오기
        mainCamera = Camera.main;

    }

    void Update()
    {
        Move1();
        //Move2();
        //Move3();
        //Move4();

        //플레이어 화면 안에 가두기
        MoveInScree();
    }

    void Move1()
    {
        //1. 입력받기 (키보드, 마우스 등 입력은 Inpuit매니저가 담당한다.)
        // Input.GetAxis("Horizontal") -> -1~ 1     (실수) -1,... ,-0.3, ... 0, 0.6, 1
        // Input.GetAxisRaw("Horizontal") -> -1~1   (정수) -1, 0, 1

        float moveX = Input.GetAxis("Horizontal"); //좌우 입력
        float moveY = Input.GetAxis("Vertical");   //좌우 입력

        //2. 이동 벡터 만들기.
        Vector3 dir = new Vector3(moveX, moveY, 0); //z축은 0으로 고정

        //3. 방향 벡터 정규화 (대각선 이동시 속도 보정.)
        //dir = dir.normalized;
        dir.Normalize();

        //4. 이동해라
        //Vector3 pos = transform.position + dir * speed* Time.deltaTime;
        //transform.position = pos;

        transform.position += dir * speed * Time.deltaTime;



    }
    void Move2()
    {
        float moveX = Input.GetAxis("Horizontal"); //좌우 입력
        float moveY = Input.GetAxis("Vertical");   //좌우 입력

        Vector3 dir = new Vector3(moveX, moveY, 0);//z축은 0으로 고정
        dir.Normalize();
       
        transform.Translate(dir * speed * Time.deltaTime);//디폴트 값을 월드로 되어있음.
        
        //Translate는 로컬좌표계 기준 이동
        //Space.World: 월드좌표계 기준 이동. (게임 세계의 절대 좌표)
        //Space.Self : 로컬좌표계 기준 이동. (오브젝트 자신의 좌표 - 자신의 방향 기준)
        
        //transform.Translate(dir * speed * Time.deltaTime,Space.World);

    }

    void Move3()
    {
        //MoveToWards는 현재 위치에서 목표 위치로 일정 속도로 이동
        //MoveToWards(현재위치, 목표위치, 속도*시간)

        // 1. 입력받기
        float moveX = Input.GetAxis("Horizontal"); //좌우 입력
        float moveY = Input.GetAxis("Vertical");   //좌우 입력

        // 2. 입력이 있으면 목표 위치 갱신
        if(moveX != 0f || moveY != 0f)
        {
            Vector3 dir = new Vector3(moveX, moveY, 0f);
            dir.Normalize();
            targetPosition += dir * speed * Time.deltaTime; //1유닛 이동한 위치를 목표위치로 설정
        }
        // 3. 현재 위치에서 목표 위치로 이동(부드럽게 이동)
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed);

    }

    void Move4()
    {
        //메인 카메라의 중요 함수
        //메인 카메라 또한 자주 사용하기 때문에 어디서든 접근할 수 있도록 변수 선언해서 사용한다.
        // Camera.main: 씬에서 "MainCamera"" 태그가 붙은 카메라를 찾아서 반환.

        // 1. ScreenToWorldPoint : 화면 좌표를 월드 좌표로 변환.
        // 2. ScreenToViewportPoint : 화면좌표를 뷰포트 좌표로 변환
        // 3. ViiewporttoWoldPoint : 뷰포트 좌표를 월드 좌표로 변환
        // 4. WorldToScreenPoint : 월드 좌표를 화면 좌표로 변환
        // 5. WorldToViewportPint : 월드좌표를 뷰포트 좌표로 변환
        // 6. ViewportToScreenPoint : 뷰포트 좌표를 화면 좌표로 변환

        //스크린의 화면을 마우스로 클릭했을 대 3D 공간의 클릭지점으로 오브젝트를 움직일때
        // Camera.main.ScreenTopWorldPoint(Input.mousePointion);
        
        // 마우스 왼족 버튼을 누르고 있는 동안
        if (Input.GetMouseButton(0)) 
        {
            //마우스 스크린 좌표를 월드 좌표로 변환
            //플레이어 높이(z축)는 유지해 줘야 한다.
            Vector3 mousPos = Input.mousePosition;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousPos);
            worldPos.z = transform.position.z;//플레이어 높이 유지

            print("마우스 클릭 좌표 : " + mousPos);
            print("월드 좌표 : " + worldPos);
            print("플레이어 월드 좌표 : " + transform.position);
            //만약 원 클릭으로 클릭좌표까지 이동하려면 아래  MoveTowards()함수가 업데이트에
            transform.position = Vector3.MoveTowards(
                transform.position,
                worldPos,
                speed * Time.deltaTime
                );

        }
    }

    void MoveInScree()
    {
        //플레이어를 화면 밖으로 나가지 못하게 막기
        // 1. 화면 밖 공간에 큐브 4개 만들억서 배치하면 충돌체 때문에 밖으로 벗어나지 못한다.
        // 2. 플레이어 트랜스 폼의 포지션 x, y값을 고정시킨다.

        //Vector3 position = transform.position;
        //if (position.x > 2.5f) position.x = 2.5f;
        //if (position.y < 2.5f) position.y = -2.5f;

        //유니티에서는 클램프 사용이 더 권장된다 (성능차이 없음.)
        //position.x = Mathf.Clamp(position.x, -2.5f, 2.5f);
        //position.y = Mathf.Clamp(position.x, -2.5f, 2.5f);

        //3. 메인카메라의 뷰포틀흘 가져와서 처리한다.
        //스크린좌표: 모니터 해상도의 픽셀.
        //뷰포트좌표: 카메라의 사각뿔의 (왼쪽 하단 : (0,0) , 우측 상단 : (1,1) )
        //UV좌표 : 화변 텍스트, 2D이미즐르 포시하기 위하한 좌표계(텍스쳐 좌표계라고도 한다.)
        //왼쪽상단 (0,0), 우측하단(1,1)

        Vector3 position = mainCamera.WorldToViewportPoint(transform.position);
        position.x = Mathf.Clamp(position.x,0f + paddingX,1f - paddingX);
        position.y = Mathf.Clamp(position.y, 0f + paddingY, 1f - paddingY);
        transform.position = mainCamera.ViewportToWorldPoint(position);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //float horizontalMove = Input.GetAxis("Horizontal");
    //    //float verticalMove = Input.GetAxis("Vertical");

    //    //if (horizontalMove >0)
    //    //{
    //    //    transform.Translate(Vector3.left *Time.deltaTime* speed);
    //    //}
    //    //if (horizontalMove < 0)
    //    //{
    //    //    transform.Translate(Vector3.right * Time.deltaTime * speed);
    //    //}

    //    //if(verticalMove > 0)
    //    //{
    //    //    transform.Translate(Vector3.up * Time.deltaTime * speed);
    //    //}
    //    //if (verticalMove < 0)
    //    //{
    //    //    transform.Translate(Vector3.down * Time.deltaTime * speed);
    //    //}
    //}
}
