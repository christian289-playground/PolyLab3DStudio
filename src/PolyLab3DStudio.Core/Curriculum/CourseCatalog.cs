namespace PolyLab3DStudio.Core;

/// <summary>
/// The learning courses with every lesson, mission, quiz, and reading page.
/// Courses c1-c4 and tracks A/B are transcribed verbatim from the studio design.
/// Course c0 (toolchain setup) and track C (Blender to Stride) were added
/// afterwards: they cover work done outside the app, so their lessons are
/// reading-only — the task checker cannot observe external tools.
/// </summary>
public static class CourseCatalog
{
    public static IReadOnlyList<Course> All { get; } = Build();

    public static Course? Find(string courseId) => All.FirstOrDefault(c => c.Id == courseId);

    public static Lesson? GetLesson(string courseId, int index)
    {
        Course? course = Find(courseId);
        return course is not null && index >= 0 && index < course.Lessons.Count ? course.Lessons[index] : null;
    }

    public static string LessonKey(string courseId, Lesson lesson) => $"{courseId}.{lesson.Id}";

    private static IReadOnlyList<Course> Build() =>
    [
        new(
            Id: "c0", Num: "00", Title: "준비물 — 새 PC에서 시작하기", English: "SETTING UP ON A NEW PC",
            Description: "다른 PC에서 폴리랩으로 3D 교육을 시작할 때 무엇을 설치해야 하는지 정리한 코스예요. 결론부터 말하면 코스 01~04와 트랙 A·B는 추가 설치가 필요 없고, 외부 도구는 트랙 C에서만 씁니다.",
            Lessons:
            [
                new(
                    Id: "l1", Kind: LessonKind.Reading, Title: "무엇이 필요하고, 무엇이 필요 없나", English: "WHAT YOU ACTUALLY NEED", Minutes: 5,
                    Pages:
                    [
                        new(
                            "설치가 필요 없는 구간이 대부분이에요",
                            [
                                "폴리랩이 실행되는 PC라면 코스 01~04(3D 공간·변형 3총사·재질과 빛·WPF로 3D 그리기)와 트랙 A(순정 WPF 3D 심화)를 처음부터 끝까지 진행할 수 있어요. 스튜디오·3D 사전·도구 지도도 마찬가지입니다. 외부 프로그램은 하나도 필요하지 않아요.",
                                "트랙 B(DirectX 사다리)는 읽기 자료와 퀴즈로만 되어 있어요. 코드를 눈으로 읽고 판단 기준을 익히는 구간이라, 역시 아무것도 설치하지 않고 끝까지 갈 수 있습니다.",
                                "실제로 다른 프로그램이 필요한 곳은 트랙 C 하나예요. Blender로 모델을 만들고 Stride로 옮기는 실습이라 두 도구를 설치해야 합니다. 그 설치 절차가 이 코스의 나머지 레슨입니다.",
                                "내 PC에 무엇이 이미 깔려 있는지는 직접 확인하지 않아도 돼요. 설정 화면(시작 화면 오른쪽 위 SETTINGS)을 열면 환경 점검 카드가 있고, winget · .NET SDK · Blender · Stride 런처를 찾았는지 한눈에 보여줍니다. 도구를 설치한 뒤에는 다시 확인 버튼을 누르면 상태가 갱신돼요.",
                            ]),
                        new(
                            "폴리랩 자체를 새 PC에 올리기",
                            [
                                "폴리랩은 .NET 10 기반 WPF 앱이라 Windows에서만 돌아가요. 소스에서 직접 빌드해 쓸 거면 .NET 10 SDK가 필요하고, 학습자에게 완성된 실행 파일만 나눠 줄 거면 .NET 10 데스크톱 런타임만 있으면 됩니다.",
                                "교실을 준비하는 입장이라면 후자가 훨씬 편해요. 학습자 PC마다 SDK와 소스를 두는 대신, 강사 PC에서 한 번 빌드한 결과물과 런타임만 배포하면 됩니다. SDK는 약 1GB에 가깝고 런타임은 훨씬 작아요.",
                                "런타임이 없는 PC에서 실행 파일을 열면 .NET을 설치하라는 안내 창이 뜹니다. 당황할 일은 아니지만, 교육 중에 마주치면 흐름이 끊기니 미리 깔아 두는 편이 좋아요.",
                            ],
                            """
                            # 소스로 빌드해 쓸 때 (강사 PC)
                            winget install -e --id Microsoft.DotNet.SDK.10
                            
                            dotnet --info                                   # 설치 확인

                            dotnet build PolyLab3DStudio.slnx
                            
                            dotnet run --project src/PolyLab3DStudio.WpfApp/PolyLab3DStudio.WpfApp.csproj

                            # 실행 파일만 배포할 때 (학습자 PC)
                            winget install -e --id Microsoft.DotNet.DesktopRuntime.10
                            """,
                            Commands: true),
                    ]),
                new(
                    Id: "l2", Kind: LessonKind.Reading, Title: "winget 먼저 확인하기", English: "CHECK WINGET FIRST", Minutes: 6,
                    Pages:
                    [
                        new(
                            "winget이 있는 PC인지 확인",
                            [
                                "winget은 Windows에 기본 탑재된 패키지 관리자예요. 앱 설치 관리자(App Installer)의 일부로 들어 있어서, Windows 11이면 대개 그냥 있습니다. Windows 10에서는 없을 수도 있어요.",
                                "터미널을 여는 방법부터 — 키보드에서 Windows 키와 X를 함께 누르면 메뉴가 뜨고, 거기서 터미널을 고르면 됩니다. 시작 메뉴에서 terminal 이라고 검색해도 같아요. 검은 창이 열리면 준비된 것입니다.",
                                "그 창에 winget --version 을 입력하고 Enter를 누르세요. 버전 번호가 나오면 준비가 끝난 것이고, 명령을 찾을 수 없다는 오류가 나면 Microsoft Store에서 앱 설치 관리자를 설치하면 됩니다.",
                                "winget이 준비되면 그다음부터는 도구 이름을 몰라도 됩니다. 이 코스가 알려주는 패키지 ID를 그대로 붙여 넣으면 되니까요.",
                            ],
                            """
                            winget --version
                            
                            # > v1.29.280      ← 이런 식으로 나오면 준비 완료

                            # 오류가 난다면: Microsoft Store에서 "앱 설치 관리자" 검색해 설치
                            # (패키지 ID: Microsoft.AppInstaller)
                            """,
                            Commands: true),
                        new(
                            "여러 PC를 똑같이 맞추는 옵션",
                            [
                                "교실에서는 PC마다 버전이 다르면 곤란해요. 화면 구성이 달라지고, 어떤 PC에서만 되는 기능이 생기면 수업이 멈춥니다. -e 옵션으로 패키지 ID를 정확히 일치시키고 --version 으로 버전을 고정하면 어느 PC에서든 같은 것이 설치됩니다.",
                                "무인 설치 플래그를 붙이면 확인 프롬프트 없이 진행돼서 여러 대를 순서대로 세팅할 때 좋아요. --scope 로 사용자 단위와 기기 단위를 고를 수 있고, 기기 단위 설치는 관리자 권한이 필요합니다.",
                            ],
                            """
                            # 버전을 고정해 모든 PC를 동일하게
                            winget install -e --id BlenderFoundation.Blender.LTS.4.5 --version 4.5.10

                            # 프롬프트 없이 조용히 설치 (여러 대 세팅용)
                            winget install -e --id BlenderFoundation.Blender --silent --accept-package-agreements --accept-source-agreements

                            # 기기 단위 설치 (관리자 권한 필요)
                            winget install -e --id Microsoft.DotNet.SDK.10 --scope machine
                            """,
                            Commands: true),
                        new(
                            "winget을 쓸 수 없는 환경이라면",
                            [
                                "학교나 회사 PC는 정책으로 winget이나 Microsoft Store가 막혀 있을 수 있어요. 그럴 때도 길은 남아 있습니다 — 각 도구의 공식 사이트에서 설치 파일을 직접 받으면 돼요. Blender는 blender.org, Stride는 stride3d.net, .NET은 dotnet.microsoft.com입니다.",
                                "관리자 권한이 아예 없는 PC라면 Blender의 포터블(zip) 배포가 가장 확실해요. 설치 과정 없이 압축만 풀고 blender.exe를 실행하면 됩니다. USB에 담아 여러 PC를 돌 수도 있어요.",
                                "다만 Stride에는 이런 무설치 방식이 없습니다. Stride까지 실습해야 하는데 권한이 없다면, 강사 PC 한 대에서 시연하는 방식으로 계획을 잡는 편이 현실적이에요.",
                            ]),
                    ]),
                new(
                    Id: "l3", Kind: LessonKind.Reading, Title: "Blender 설치와 첫 실행 확인", English: "INSTALLING BLENDER", Minutes: 8,
                    Pages:
                    [
                        new(
                            "최신판이냐 LTS냐",
                            [
                                "winget에는 Blender 최신판과 LTS(장기 지원)판이 따로 올라와 있어요. 혼자 배우는 중이라면 최신판으로 충분하고, 여러 PC를 맞춰야 하거나 교재 화면과 똑같이 보이는 것이 중요하다면 LTS를 고르세요.",
                                "LTS는 큰 변화 없이 오래 유지되는 버전이라, 수업 중간에 UI가 바뀌어 당황할 일이 적어요. 설치 후에는 winget list 로 실제 설치된 버전을 확인하는 습관을 들이면 좋습니다.",
                                "설치가 끝나면 폴리랩의 설정 화면에서 환경 점검 카드의 다시 확인을 눌러 보세요. Blender 줄이 확인됨으로 바뀌고 찾아낸 blender.exe의 전체 경로가 표시됩니다 — 다음 페이지에서 쓸 경로가 바로 그것이에요.",
                            ],
                            """
                            winget install -e --id BlenderFoundation.Blender           # 최신판
                            
                            winget install -e --id BlenderFoundation.Blender.LTS.4.5   # LTS

                            winget list --id BlenderFoundation.Blender                  # 설치 확인
                            """,
                            Commands: true),
                        new(
                            "PATH에 등록되지 않는다는 함정",
                            [
                                "winget으로 설치해도 blender.exe는 PATH에 등록되지 않아요. 터미널에서 blender 라고 치면 명령을 찾을 수 없다는 오류가 납니다. 설치가 잘못된 게 아니라 원래 그렇습니다.",
                                "메뉴에서 아이콘을 눌러 GUI로만 쓸 거면 아무 문제가 없어요. 하지만 트랙 C에서 파이썬 스크립트를 화면 없이(headless) 실행할 때는 blender.exe의 전체 경로가 필요합니다.",
                                "설치 위치는 보통 C:\\Program Files\\Blender Foundation\\Blender <버전> 폴더예요. 버전 번호가 폴더 이름에 들어가니, Blender를 올린 뒤에는 경로도 함께 바뀐다는 점을 기억하세요.",
                            ],
                            """
                            where.exe blender
                            
                            # > 지정된 파일을 찾을 수 없습니다   ← PATH에 없는 것이 정상

                            # 전체 경로로 버전 확인
                            "C:\Program Files\Blender Foundation\Blender 5.2\blender.exe" --version

                            # 화면 없이 스크립트만 실행 (트랙 C에서 사용)
                            "C:\Program Files\Blender Foundation\Blender 5.2\blender.exe" --background --python make_tower.py
                            """,
                            Commands: true),
                        new(
                            "첫 실행에서 확인할 세 가지",
                            [
                                "하나, 처음 열면 스플래시 화면에 버전이 표시돼요. 교실 전체가 같은 숫자인지 여기서 확인하는 게 가장 빠릅니다.",
                                "둘, 상단 탭 맨 오른쪽에 Scripting이 있는지 보세요. 트랙 C에서 파이썬을 쓰는 화면이고, 없으면 버전이 너무 낮은 것입니다.",
                                "셋, 뷰포트가 검게 나오거나 실행 직후 종료되면 대개 그래픽 드라이버나 GPU 문제예요. Blender는 비교적 최신 그래픽 API를 요구하므로 오래된 내장 GPU에서는 드라이버 업데이트가 먼저입니다. 정확한 요구사항은 blender.org의 download/requirements 페이지에서 확인하세요.",
                                "미리 알아 두면 좋은 것 하나 — Blender는 Z가 위인 좌표계를 씁니다. 폴리랩과 WPF는 Y가 위예요. 3D 사전의 축 항목에 정리되어 있고, 트랙 C에서 모델이 누워서 들어오는 이유가 바로 이 차이입니다.",
                            ]),
                    ]),
                new(
                    Id: "l4", Kind: LessonKind.Reading, Title: "Stride 설치 (트랙 C 후반에만 필요)", English: "INSTALLING STRIDE", Minutes: 6,
                    Pages:
                    [
                        new(
                            "winget에 없는 도구",
                            [
                                "Stride만은 winget으로 설치할 수 없어요. winget search stride 를 실행하면 이름만 비슷한 무관한 프로그램이 나올 뿐입니다. 공식 배포 경로는 stride3d.net의 StrideSetup.exe(약 55MB)예요.",
                                "그리고 StrideSetup.exe가 설치하는 것은 엔진이 아니라 Stride 런처입니다. 런처를 실행해 엔진 버전을 골라 설치하면 씬 에디터인 Game Studio가 함께 깔려요. 즉 설치가 두 단계입니다 — 런처를 깔고, 런처로 엔진을 깔아요.",
                                "엔진 본체는 용량이 크니 디스크 여유를 미리 확인하세요. 그리고 Stride는 Windows 전용이에요(다른 OS는 실험 단계). 설치 안내 문서는 doc.stride3d.net의 get-started/install-stride 페이지입니다.",
                            ],
                            """
                            winget search stride
                            
                            # > mousestride   trustbe.mousestride   ← 전혀 다른 프로그램

                            # 대신 stride3d.net/download 에서 StrideSetup.exe (약 55MB)

                            # 런처가 설치되는 위치
                            %LOCALAPPDATA%\Programs\Stride\Stride.Launcher.exe

                            # 런처 실행 → 엔진 버전 선택 설치 → Game Studio 열기
                            """,
                            Commands: true),
                        new(
                            "여기까지 안 해도 되는 경우",
                            [
                                "교육 일정이 짧다면 Stride는 건너뛸 수 있어요. 트랙 C의 앞 두 레슨(파이썬으로 모델 만들기, 파일로 내보내기)은 Blender만 있으면 끝까지 진행됩니다. 그 두 레슨만으로도 3D 파일 형식과 축·단위 문제를 충분히 겪을 수 있어요.",
                                "스크립트를 컴파일하는 것 자체는 .NET SDK만으로도 됩니다. 하지만 씬에 모델을 올리고 스크립트를 컴포넌트로 붙이는 일은 Game Studio에서 해야 해요. 그래서 트랙 C의 마지막 레슨을 학습자가 직접 실습하려면 런처 설치를 건너뛸 수 없습니다.",
                                "권한이나 시간이 부족하면 마지막 레슨만 강사 PC에서 시연하는 방식도 좋은 타협이에요. 파이프라인의 결말을 눈으로 보는 것만으로도 앞 레슨들의 이유가 분명해집니다.",
                            ]),
                    ]),
                new(
                    Id: "q", Kind: LessonKind.Quiz, Title: "개념 체크: 준비물", English: "QUIZ", Minutes: 4,
                    Questions:
                    [
                        new("코스 01~04와 트랙 A를 진행하려면 추가로 설치해야 하는 프로그램은?", ["없음 — 폴리랩만으로 끝까지 진행된다", "Blender", "Stride"], 0,
                            "외부 도구가 필요한 구간은 트랙 C 하나뿐이에요."),
                        new("새 PC에 winget이 있는지 확인하는 명령은?", ["winget --version", "winget check", "winget --exists"], 0,
                            "버전이 나오면 준비 완료, 오류가 나면 Microsoft Store에서 앱 설치 관리자를 설치하세요."),
                        new("여러 PC에 똑같은 Blender 버전을 설치하려면?", ["-e로 ID를 정확히 맞추고 --version으로 버전을 고정", "설치할 때마다 최신판을 받는다", "PC마다 다른 버전을 써도 무관하다"], 0,
                            "교실에서 화면이 서로 다르면 수업이 멈춰요. LTS + 버전 고정이 안전합니다."),
                        new("winget으로 Blender를 설치한 뒤 터미널에서 blender 명령이 동작하지 않는 이유는?", ["PATH에 등록되지 않아서 — 전체 경로를 써야 한다", "설치가 실패한 것이다", "관리자 권한이 없어서"], 0,
                            "정상 동작이에요. headless 실행에는 blender.exe의 전체 경로가 필요합니다."),
                        new("학습자 PC에 완성된 폴리랩 실행 파일만 배포할 때 필요한 것은?", [".NET 10 데스크톱 런타임", ".NET 10 SDK", "Visual Studio"], 0,
                            "SDK는 빌드하는 강사 PC에만 있으면 되고, 실행에는 런타임만 필요해요."),
                    ]),
            ]),
        new(
            Id: "c1", Num: "01", Title: "3D 공간과 친해지기", English: "MEET THE 3D SPACE",
            Description: "3D 작업의 절반은 \"잘 보는 것\"이에요. 카메라를 자유롭게 움직이고, 첫 오브젝트를 만들어 봅니다.",
            Lessons:
            [
                new(
                    Id: "l1", Kind: LessonKind.Lesson, Title: "카메라 움직이기", English: "MOVING THE CAMERA", Minutes: 5, Seed: "single",
                    Tip: "길을 잃었다면 화면 오른쪽 위 [입체] 버튼을 누르세요. 처음 시점으로 돌아옵니다.",
                    Tasks:
                    [
                        new("뷰포트를 드래그해서 시점을 빙 돌려 보세요", new TaskCheck("action", Action: "orbit")),
                        new("마우스 휠로 확대/축소해 보세요", new TaskCheck("action", Action: "zoom")),
                        new("오른쪽 버튼으로 드래그해서 화면을 옮겨 보세요", new TaskCheck("action", Action: "pan")),
                        new("오른쪽 위 [앞]이나 [위] 버튼으로 시점을 바꿔 보세요", new TaskCheck("action", Action: "preset")),
                    ]),
                new(
                    Id: "l2", Kind: LessonKind.Lesson, Title: "만들고, 고르고, 지우기", English: "CREATE · SELECT · DELETE", Minutes: 6, Seed: "empty",
                    Tip: "빈 곳을 클릭하면 선택이 풀려요. 왼쪽 [장면] 목록에서도 선택할 수 있어요.",
                    Tasks:
                    [
                        new("위 [만들기]에서 큐브를 추가해 보세요", new TaskCheck("add", Shape: ShapeKind.Cube)),
                        new("이번엔 구(Sphere)를 하나 추가해 보세요", new TaskCheck("add", Shape: ShapeKind.Sphere)),
                        new("오브젝트를 클릭해서 선택해 보세요", new TaskCheck("select")),
                        new("선택한 오브젝트를 삭제해 보세요 (Delete 키)", new TaskCheck("delete")),
                    ]),
                new(
                    Id: "q", Kind: LessonKind.Quiz, Title: "개념 체크: 3D 공간", English: "QUIZ", Minutes: 3,
                    Questions:
                    [
                        new("3D 공간에서 위·아래(높이)를 나타내는 축은 무엇일까요?", ["X축", "Y축", "Z축"], 1,
                            "폴리랩에서는 초록색 Y축이 위쪽 방향이에요. X는 좌우, Z는 앞뒤를 나타냅니다."),
                        new("마우스 휠을 굴리면 어떤 일이 일어나나요?", ["시점이 회전한다", "화면이 확대/축소된다", "오브젝트가 복제된다"], 1,
                            "휠은 카메라를 가까이/멀리 옮기는 확대·축소(Zoom)예요."),
                        new("오브젝트를 선택하는 가장 기본적인 방법은?", ["빈 공간을 드래그한다", "오브젝트를 클릭한다", "휠을 꾹 누른다"], 1,
                            "클릭 한 번이면 선택! 빈 곳을 클릭하면 선택이 풀립니다."),
                    ]),
            ]),
        new(
            Id: "c2", Num: "02", Title: "변형 3총사: 이동·회전·크기", English: "MOVE · ROTATE · SCALE",
            Description: "모든 3D 도구의 심장인 세 가지 변형 도구를 익히고, 첫 작품 눈사람을 만들어 봅니다.",
            Lessons:
            [
                new(
                    Id: "l1", Kind: LessonKind.Lesson, Title: "이동 도구 (W)", English: "MOVE TOOL", Minutes: 5, Seed: "single",
                    Tip: "위로 올릴 때는 Shift를 누른 채 드래그! 대부분의 3D 도구에 비슷한 개념이 있어요.",
                    Tasks:
                    [
                        new("이동 도구를 켜 보세요 (W 키 또는 툴바)", new TaskCheck("tool", Tool: ToolCatalog.Move)),
                        new("큐브를 드래그해서 바닥 위에서 옮겨 보세요", new TaskCheck("transform", Mode: "move")),
                        new("Shift를 누른 채 드래그해서 공중으로 띄워 보세요", new TaskCheck("transform", Mode: "move", Shift: true)),
                    ]),
                new(
                    Id: "l2", Kind: LessonKind.Lesson, Title: "회전과 크기 (E · R)", English: "ROTATE & SCALE", Minutes: 5, Seed: "single",
                    Tip: "오른쪽 속성 패널에서 숫자로 정확하게 조절할 수도 있어요.",
                    Tasks:
                    [
                        new("회전 도구를 켜 보세요 (E)", new TaskCheck("tool", Tool: ToolCatalog.Rotate)),
                        new("오브젝트를 좌우로 드래그해서 돌려 보세요", new TaskCheck("transform", Mode: "rotate")),
                        new("크기 도구를 켜 보세요 (R)", new TaskCheck("tool", Tool: ToolCatalog.Scale)),
                        new("오브젝트를 위아래로 드래그해서 크기를 바꿔 보세요", new TaskCheck("transform", Mode: "scale")),
                    ]),
                new(
                    Id: "l3", Kind: LessonKind.Mission, Title: "미션: 눈사람 만들기", English: "MISSION: SNOWMAN", Minutes: 8, Seed: "empty",
                    Tip: "몸통 → 머리 → 코 순서가 편해요. 흰색은 재질 색에서 고를 수 있어요.",
                    Tasks:
                    [
                        new("구를 3개 만들어 보세요", new TaskCheck("count", Shape: ShapeKind.Sphere, Count: 3)),
                        new("크기 도구(R)로 구들의 크기를 서로 다르게 해 보세요", new TaskCheck("transform", Mode: "scale")),
                        new("이동 도구(W) + Shift 드래그로 위로 쌓아 보세요", new TaskCheck("transform", Mode: "move", Shift: true)),
                        new("원뿔을 추가해서 코를 달아 주세요", new TaskCheck("add", Shape: ShapeKind.Cone)),
                        new("속성 패널에서 마음에 드는 색을 입혀 보세요", new TaskCheck("color")),
                    ]),
                new(
                    Id: "q", Kind: LessonKind.Quiz, Title: "개념 체크: 변형", English: "QUIZ", Minutes: 3,
                    Questions:
                    [
                        new("단축키 W는 어떤 도구인가요?", ["선택", "이동", "회전"], 1,
                            "Q 선택, W 이동, E 회전, R 크기 — 많은 3D 도구가 비슷한 배치를 써요."),
                        new("오브젝트를 공중으로 띄우려면 이동 중에 무엇을 누르나요?", ["Shift", "Ctrl", "Tab"], 0,
                            "Shift+드래그가 위아래 이동이에요."),
                        new("크기 도구의 단축키는?", ["E", "R", "S"], 1,
                            "R(크기)! E는 회전이었죠."),
                    ]),
            ]),
        new(
            Id: "c3", Num: "03", Title: "재질과 빛", English: "MATERIALS & LIGHT",
            Description: "같은 모양도 재질과 빛에 따라 완전히 달라 보여요. 색·거칠기·금속성, 그리고 조명을 다뤄 봅니다.",
            Lessons:
            [
                new(
                    Id: "l1", Kind: LessonKind.Lesson, Title: "색과 재질", English: "COLOR & MATERIAL", Minutes: 5, Seed: "two",
                    Tip: "거칠기(Roughness)가 낮을수록 매끈하고 반짝여요. 금속성(Metalness)을 올리면 금속처럼 비칩니다.",
                    Tasks:
                    [
                        new("오브젝트 하나를 선택해 보세요", new TaskCheck("select")),
                        new("속성 패널에서 색을 바꿔 보세요", new TaskCheck("color")),
                        new("거칠기 슬라이더를 움직여 보세요", new TaskCheck("slider", Key: "roughness")),
                        new("금속성 슬라이더도 움직여 보세요", new TaskCheck("slider", Key: "metalness")),
                    ]),
                new(
                    Id: "l2", Kind: LessonKind.Lesson, Title: "빛 다루기", English: "WORKING WITH LIGHT", Minutes: 4, Seed: "lit",
                    Tip: "그림자가 어느 쪽으로 지는지 살펴보세요. 빛의 방향이 장면의 분위기를 만듭니다.",
                    Tasks:
                    [
                        new("조명 [밝기] 슬라이더를 움직여 보세요", new TaskCheck("light", Key: "intensity")),
                        new("조명 [방향] 슬라이더로 그림자를 옮겨 보세요", new TaskCheck("light", Key: "angle")),
                        new("시점을 돌려서 여러 각도에서 살펴 보세요", new TaskCheck("action", Action: "orbit")),
                    ]),
                new(
                    Id: "q", Kind: LessonKind.Quiz, Title: "개념 체크: 재질과 빛", English: "QUIZ", Minutes: 3,
                    Questions:
                    [
                        new("거칠기(Roughness)를 낮추면 표면은 어떻게 보일까요?", ["더 반짝인다", "더 어두워진다", "더 커진다"], 0,
                            "거칠기가 낮다 = 표면이 매끈하다 = 빛을 잘 반사해서 반짝여요."),
                        new("금속성(Metalness)을 최대로 올리면?", ["투명해진다", "금속처럼 보인다", "그림자가 사라진다"], 1,
                            "금속성은 표면이 금속처럼 빛을 반사하는 정도예요."),
                        new("빛의 방향을 바꾸면 함께 움직이는 것은?", ["그림자", "오브젝트", "카메라"], 0,
                            "빛의 반대편으로 그림자가 드리워져요."),
                    ]),
            ]),
        new(
            Id: "c4", Num: "04", Title: "WPF로 3D 그리기", English: "WPF 3D FOR .NET DEVELOPERS",
            Description: "폴리랩에서 배운 개념이 .NET/WPF 코드로 어떻게 옮겨지는지 배워요. Viewport3D부터 Transform3DGroup까지, 그리고 내 장면을 직접 코드로 내보내 봅니다.",
            Lessons:
            [
                new(
                    Id: "r1", Kind: LessonKind.Reading, Title: "WPF 3D의 뼈대", English: "VIEWPORT3D & SCENE TREE", Minutes: 5,
                    Pages:
                    [
                        new(
                            "Viewport3D — 3D가 그려지는 창",
                            [
                                "WPF에서 3D는 Viewport3D라는 컨트롤 안에 그려져요. 폴리랩 가운데의 뷰포트가 바로 이 역할이에요.",
                                "Viewport3D는 평범한 XAML 컨트롤이라 Grid 안에 넣고, 버튼·패널과 함께 배치할 수 있어요. 별도 라이브러리 없이 .NET에 기본 포함되어 있습니다.",
                            ],
                            """
                            <Grid>
                                <Viewport3D>
                                    <Viewport3D.Camera>
                                        <PerspectiveCamera x:Name="Camera" FieldOfView="50"/>
                                    </Viewport3D.Camera>
                                    <ModelVisual3D x:Name="SceneRoot"/>
                                </Viewport3D>
                            </Grid>
                            """),
                        new(
                            "장면은 트리 — ModelVisual3D",
                            [
                                "폴리랩 왼쪽의 [장면] 목록처럼, WPF 3D도 ModelVisual3D 노드를 트리로 쌓아서 장면을 만들어요.",
                                "SceneRoot.Children.Add(...)를 호출하는 것이 폴리랩의 [만들기] 버튼과 정확히 같은 일이에요. 오브젝트도, 조명도 모두 이 트리의 노드입니다.",
                            ],
                            """
                            // 오브젝트 추가 = 트리에 노드 추가
                            SceneRoot.Children.Add(new ModelVisual3D { Content = model });

                            // 조명도 같은 방식!
                            SceneRoot.Children.Add(new ModelVisual3D {
                                Content = new AmbientLight(Color.FromRgb(72, 78, 86))
                            });
                            """),
                        new(
                            "카메라 — PerspectiveCamera",
                            [
                                "Position(카메라 위치), LookDirection(바라보는 방향), FieldOfView(화각) 세 가지만 알면 돼요.",
                                "폴리랩에서 드래그로 시점을 돌리는 것은, 사실 이 값들을 매 프레임 다시 계산해 넣는 것이에요. [XAML 내보내기]의 UpdateCamera()에서 실제 계산식을 볼 수 있어요.",
                            ],
                            """
                            Camera.Position = new Point3D(6, 5, 8);
                            Camera.LookDirection = new Vector3D(-6, -4.3, -8); // 원점을 향해
                            Camera.UpDirection = new Vector3D(0, 1, 0);        // Y가 위
                            """),
                    ]),
                new(
                    Id: "r2", Kind: LessonKind.Reading, Title: "메시와 재질", English: "MESHGEOMETRY3D & MATERIALS", Minutes: 5,
                    Pages:
                    [
                        new(
                            "MeshGeometry3D — 점과 삼각형",
                            [
                                "모든 입체는 정점 목록(Positions)과 삼각형 목록(TriangleIndices)으로 정의돼요. 폴리랩의 큐브도 삼각형 12개로 이루어져 있어요.",
                                "TriangleIndices는 3개씩 끊어 읽어요 — \"0, 1, 2\"는 0·1·2번 정점으로 삼각형 하나를 만들라는 뜻이에요.",
                            ],
                            """
                            var m = new MeshGeometry3D();
                            m.Positions.Add(new Point3D(0, 0, 0)); // 0번 정점
                            m.Positions.Add(new Point3D(1, 0, 0)); // 1번 정점
                            m.Positions.Add(new Point3D(0, 1, 0)); // 2번 정점
                            m.TriangleIndices.Add(0);
                            m.TriangleIndices.Add(1);
                            m.TriangleIndices.Add(2); // → 삼각형 1개 완성
                            """),
                        new(
                            "GeometryModel3D — 모양 + 재질",
                            [
                                "메시(모양)에 Material(재질)을 입힌 것이 GeometryModel3D예요. 폴리랩의 재질 패널이 여기에 대응돼요.",
                                "DiffuseMaterial은 기본 색, SpecularMaterial은 반짝임(폴리랩의 거칠기를 낮춘 효과), EmissiveMaterial은 스스로 빛나는 효과예요. MaterialGroup으로 겹쳐 쓸 수 있어요.",
                            ],
                            """
                            var mat = new MaterialGroup();
                            mat.Children.Add(new DiffuseMaterial(new SolidColorBrush(color)));
                            mat.Children.Add(new SpecularMaterial(Brushes.White, 60)); // 반짝!

                            var model = new GeometryModel3D(mesh, mat);
                            """),
                    ]),
                new(
                    Id: "r3", Kind: LessonKind.Reading, Title: "변형과 조명", English: "TRANSFORMS & LIGHTS", Minutes: 5,
                    Pages:
                    [
                        new(
                            "Transform3DGroup — W·E·R의 정체",
                            [
                                "폴리랩의 이동(W)·회전(E)·크기(R)는 WPF에서 각각 TranslateTransform3D, RotateTransform3D, ScaleTransform3D예요.",
                                "Transform3DGroup으로 묶으면 순서대로 적용돼요. 보통 크기 → 회전 → 이동 순서를 써요. 순서를 바꾸면 결과가 달라지니 주의!",
                            ],
                            """
                            var tg = new Transform3DGroup();
                            tg.Children.Add(new ScaleTransform3D(s, s, s));
                            tg.Children.Add(new RotateTransform3D(
                                new AxisAngleRotation3D(new Vector3D(0, 1, 0), 45))); // Y축 45도
                            tg.Children.Add(new TranslateTransform3D(x, y, z));
                            model.Transform = tg;
                            """),
                        new(
                            "조명 — 빛이 없으면 검은 화면",
                            [
                                "WPF 3D는 빛이 없으면 아무것도 안 보여요. DirectionalLight(한 방향에서 오는 태양빛)와 AmbientLight(전체를 은은하게 밝히는 빛)를 함께 쓰는 것이 기본이에요.",
                                "폴리랩 [조명] 패널의 밝기·방향 슬라이더가 곧 DirectionalLight의 Color와 Direction 값이에요.",
                            ],
                            """
                            // 태양빛: 방향 벡터가 "빛이 나아가는 방향"
                            SceneRoot.Children.Add(new ModelVisual3D {
                                Content = new DirectionalLight(Colors.White, new Vector3D(-1, -1, -1))
                            });
                            // 은은한 바탕 빛
                            SceneRoot.Children.Add(new ModelVisual3D {
                                Content = new AmbientLight(Color.FromRgb(72, 78, 86))
                            });
                            """),
                    ]),
                new(
                    Id: "m4", Kind: LessonKind.Mission, Title: "미션: 내 장면을 WPF 코드로", English: "MISSION: EXPORT TO WPF", Minutes: 6, Seed: "single",
                    Tip: "내보낸 코드의 BuildScene()을 보면, 방금 만든 도형들이 AddObject(...) 한 줄씩으로 나타나 있어요.",
                    Tasks:
                    [
                        new("도형을 하나 추가해 보세요 (포인트 클라우드도 좋아요)", new TaskCheck("add")),
                        new("이동·회전·크기로 마음대로 배치해 보세요", new TaskCheck("transform")),
                        new("상단의 [XAML] 버튼으로 내보내기를 열어 보세요", new TaskCheck("action", Action: "xaml")),
                        new("[MainWindow.xaml.cs] 탭에서 내 도형 코드를 확인해 보세요", new TaskCheck("action", Action: "cstab")),
                        new("[코드 복사]를 눌러 Visual Studio로 가져갈 준비!", new TaskCheck("action", Action: "copy")),
                    ]),
                new(
                    Id: "q", Kind: LessonKind.Quiz, Title: "개념 체크: WPF 3D", English: "QUIZ", Minutes: 3,
                    Questions:
                    [
                        new("3D 장면이 그려지는 WPF 컨트롤은 무엇일까요?", ["Viewport3D", "Canvas", "RenderPanel"], 0,
                            "Canvas는 2D 전용이에요. 3D는 Viewport3D 안에서만 그려집니다."),
                        new("입체의 모양(정점·삼각형)을 담는 타입은?", ["GeometryModel3D", "MeshGeometry3D", "ModelVisual3D"], 1,
                            "Mesh가 모양, GeometryModel3D는 모양+재질, ModelVisual3D는 장면 트리의 노드예요."),
                        new("이동·회전·크기를 한 오브젝트에 함께 적용하려면?", ["Transform3DGroup", "Storyboard", "MatrixStack"], 0,
                            "세 변형을 Transform3DGroup에 순서대로 담아 model.Transform에 넣어요."),
                        new("WPF 3D 장면에서 빛을 하나도 넣지 않으면?", ["기본 조명이 자동 적용된다", "화면에 아무것도 안 보인다", "컴파일 에러가 난다"], 1,
                            "빛도 장면 트리의 노드예요. DirectionalLight + AmbientLight 조합이 기본이에요."),
                    ]),
            ]),
        new(
            Id: "ta", Num: "A", Title: "트랙 A · 순수 WPF 3D 심화", English: "PURE WPF 3D DEEP-DIVE (Media3D)",
            Description: "라이브러리로 도망가기 전에, System.Windows.Media.Media3D 순정 API만으로 원리를 끝까지 겪어요. 여기서 배운 개념은 어떤 3D 도구·엔진에서도 그대로 통합니다.",
            Lessons:
            [
                new(
                    Id: "a1", Kind: LessonKind.Reading, Title: "메시를 손으로 짓기", English: "BUILD A MESH BY HAND", Minutes: 8,
                    Pages:
                    [
                        new(
                            "정점과 인덱스로 삼각형 쌓기",
                            [
                                "모든 입체는 정점(Positions)과, 정점 3개를 이어 만드는 삼각형(TriangleIndices)의 목록이에요. 큐브 하나도 삼각형 12개입니다.",
                                "중요한 규칙: 인덱스를 적는 순서(와인딩)가 면의 \"앞면\"을 결정해요. 카메라에서 볼 때 반시계 방향이 앞면이고, 반대로 적으면 후면 컬링 때문에 그 면이 투명하게 사라져요. \"면 하나가 안 보여요\"의 원인 1순위!",
                            ],
                            """
                            var m = new MeshGeometry3D();
                            m.Positions.Add(new Point3D(0, 0, 0)); // 0
                            m.Positions.Add(new Point3D(1, 0, 0)); // 1
                            m.Positions.Add(new Point3D(1, 1, 0)); // 2
                            m.Positions.Add(new Point3D(0, 1, 0)); // 3
                            // 사각형 = 삼각형 2개. 반시계 방향으로!
                            m.TriangleIndices.Add(0); m.TriangleIndices.Add(1); m.TriangleIndices.Add(2);
                            m.TriangleIndices.Add(0); m.TriangleIndices.Add(2); m.TriangleIndices.Add(3);
                            """),
                        new(
                            "노멀 — 면이 밝기를 아는 방법",
                            [
                                "노멀(Normal)은 면이 향하는 방향 벡터예요. 빛과 노멀의 각도로 밝기가 계산되기 때문에, 노멀이 틀리면 모양은 맞아도 음영이 이상해져요.",
                                "Normals를 비워 두면 WPF가 자동 계산해 줘요(부드러운 음영). 각진 큐브를 원하면 면마다 정점을 복제하고 노멀을 직접 넣어요 — 폴리랩 XAML 내보내기의 Box()가 정점을 면마다 4개씩 새로 추가하는 이유가 바로 이것!",
                            ],
                            """
                            // 자동: m.Normals를 비워 두면 WPF가 계산 (부드러운 음영)
                            // 수동: 면마다 정점을 복제하고 방향을 명시 (각진 음영)
                            m.Normals.Add(new Vector3D(0, 0, 1)); // 이 정점은 +Z를 향한다
                            """),
                        new(
                            "UV — 사진이 감기는 좌표",
                            [
                                "TextureCoordinates(UV)는 정점마다 \"이미지의 어느 지점(0~1)이 여기에 붙는가\"를 정해요. 체커보드 이미지를 입혀 보면 UV가 틀렸을 때 무늬가 늘어나거나 뒤틀리는 게 바로 보여요.",
                                "재질에 ImageBrush를 쓰려면 UV가 반드시 있어야 해요. 다음 레슨(재질의 층)에서 실제로 입혀 봅니다.",
                            ],
                            """
                            m.TextureCoordinates.Add(new Point(0, 0)); // 이미지 좌상단
                            m.TextureCoordinates.Add(new Point(1, 0)); // 우상단
                            m.TextureCoordinates.Add(new Point(1, 1)); // 우하단
                            m.TextureCoordinates.Add(new Point(0, 1)); // 좌하단
                            """),
                    ]),
                new(
                    Id: "a2", Kind: LessonKind.Reading, Title: "재질의 층", English: "MATERIAL LAYERS", Minutes: 6,
                    Pages:
                    [
                        new(
                            "MaterialGroup — 재질을 겹쳐 쌓기",
                            [
                                "DiffuseMaterial(기본색) + SpecularMaterial(반짝임) + EmissiveMaterial(자체발광)을 MaterialGroup에 담으면 위에서부터 겹쳐 계산돼요. 폴리랩의 거칠기 슬라이더를 낮추는 것이 SpecularPower를 올리는 것과 같아요.",
                                "DiffuseMaterial의 Brush에 SolidColorBrush 대신 ImageBrush를 넣으면 사진이 표면에 입혀져요 — GIMP에서 보정한 텍스처를 3D에 붙이는 다리가 됩니다(UV 필요).",
                            ],
                            """
                            var mat = new MaterialGroup();
                            mat.Children.Add(new DiffuseMaterial(
                                new ImageBrush(new BitmapImage(new Uri("checker.png", UriKind.Relative)))));
                            mat.Children.Add(new SpecularMaterial(Brushes.White, 60));
                            mat.Children.Add(new EmissiveMaterial(
                                new SolidColorBrush(Color.FromArgb(40, 255, 120, 60))));
                            """),
                        new(
                            "BackMaterial — 사라진 면 되살리기",
                            [
                                "와인딩이 뒤집힌 면, 또는 안쪽이 보이는 열린 메시(컵, 튜브)는 뒷면이 그려지지 않아 구멍처럼 보여요.",
                                "GeometryModel3D.BackMaterial에 재질을 넣으면 뒷면도 그려져요. 다만 삼각형을 두 번 그리는 셈이라 비용이 두 배 — 필요한 모델에만 쓰세요.",
                            ],
                            """
                            var model = new GeometryModel3D(mesh, mat)
                            {
                                BackMaterial = mat // 뒷면도 같은 재질로
                            };
                            """),
                    ]),
                new(
                    Id: "a3", Kind: LessonKind.Reading, Title: "조명 4종 완전체", English: "ALL FOUR LIGHTS", Minutes: 6,
                    Pages:
                    [
                        new(
                            "PointLight와 SpotLight",
                            [
                                "지금까지 쓴 DirectionalLight(태양)·AmbientLight(바탕빛)에 더해, PointLight는 전구처럼 한 점에서 사방으로 퍼지고(위치·감쇠·범위), SpotLight는 손전등처럼 원뿔로 비춰요(방향·내부/외부 원뿔각).",
                                "넷 다 그림자는 만들지 않아요. WPF 3D에는 그림자 매핑이 없어서, 바닥에 어두운 원판을 깔거나 미리 구운 텍스처로 흉내 내는 것이 순정의 한계입니다.",
                            ],
                            """
                            new PointLight(Colors.White, new Point3D(0, 3, 0))
                            {
                                Range = 10,                 // 이 거리 밖은 무시
                                ConstantAttenuation = 1,    // 감쇠 곡선
                                LinearAttenuation = 0.1
                            };
                            new SpotLight(Colors.White, new Point3D(0, 5, 0),
                                new Vector3D(0, -1, 0), /*outerCone*/ 30, /*innerCone*/ 20);
                            """),
                        new(
                            "빛도 공짜가 아니다",
                            [
                                "WPF는 조명 계산이 무거워지면 하드웨어 렌더링을 포기하고 소프트웨어 렌더링으로 후퇴해요. 대략 Directional 110개 / Point 70개 / Spot 40개 언저리가 절벽 — 넘는 순간 FPS가 뚝 떨어집니다.",
                                "실무 수칙: 조명은 2~4개로 설계하고, 밝기가 부족하면 조명을 늘리는 대신 AmbientLight를 올리거나 EmissiveMaterial로 보완하세요.",
                            ]),
                    ]),
                new(
                    Id: "a4", Kind: LessonKind.Reading, Title: "변형 심화 — 순서·짐벌락·행렬", English: "TRANSFORMS DEEP-DIVE", Minutes: 7,
                    Pages:
                    [
                        new(
                            "순서가 결과를 바꾼다",
                            [
                                "Transform3DGroup은 담긴 순서대로 적용돼요. [크기→회전→이동]과 [이동→회전]은 완전히 다른 결과 — 후자는 물체가 원점 주위를 공전해 버려요.",
                                "표준 순서는 크기 → 회전 → 이동. 폴리랩의 XAML 내보내기가 항상 이 순서로 코드를 만드는 이유예요.",
                            ],
                            """
                            // (1) 제자리에서 돌고 → 옮겨진다 (기대한 결과)
                            tg.Children.Add(new RotateTransform3D(rot));
                            tg.Children.Add(new TranslateTransform3D(3, 0, 0));

                            // (2) 옮겨진 뒤 돌면 → 원점 주위를 공전!
                            tg.Children.Add(new TranslateTransform3D(3, 0, 0));
                            tg.Children.Add(new RotateTransform3D(rot));
                            """),
                        new(
                            "짐벌락과 쿼터니언",
                            [
                                "X·Y·Z 각도(오일러 각)로 회전을 쌓다 보면 두 축이 겹쳐 자유도 하나를 잃는 짐벌락이 생겨요. 애니메이션 중 회전이 이상하게 휙 도는 원인이에요.",
                                "QuaternionRotation3D는 회전을 4차원 수 하나로 표현해 짐벌락이 없고, 두 회전 사이 보간도 매끄러워요. 카메라·연속 회전 애니메이션에는 쿼터니언을 쓰세요.",
                                "성능 팁: 애니메이션하지 않는 오브젝트라면 Transform3DGroup 대신 행렬을 미리 곱해 MatrixTransform3D 하나로 합치면 더 가벼워요.",
                            ],
                            """
                            var q = new QuaternionRotation3D(
                                new Quaternion(new Vector3D(0, 1, 0), 45));
                            model.Transform = new RotateTransform3D(q);
                            """),
                    ]),
                new(
                    Id: "a5", Kind: LessonKind.Reading, Title: "계층 구조 — 장면 그래프", English: "SCENE GRAPH", Minutes: 5,
                    Pages:
                    [
                        new(
                            "Model3DGroup — 부모를 돌리면 자식이 딸려온다",
                            [
                                "Model3DGroup 안에 Model3DGroup을 중첩하면 부모-자식 계층이 생겨요. 부모의 Transform이 자식 전체에 곱해지므로, 차체를 돌리면 바퀴 4개가 함께 돌아요.",
                                "자식의 Transform은 부모 기준의 로컬 좌표예요. \"바퀴는 차체에서 앞으로 1.2, 아래로 0.3\" 같은 상대 배치가 가능해지는 것 — 모든 3D 도구의 부모-자식(Parenting) 개념이 이것입니다.",
                            ],
                            """
                            var wheel = new GeometryModel3D(wheelMesh, mat)
                            { Transform = new TranslateTransform3D(1.2, -0.3, 0.8) }; // 차체 기준

                            var car = new Model3DGroup();
                            car.Children.Add(bodyModel);
                            car.Children.Add(wheel); // ×4
                            car.Transform = new RotateTransform3D(rot); // 차 전체가 회전
                            """),
                    ]),
                new(
                    Id: "a6", Kind: LessonKind.Reading, Title: "카메라 투영 — 원근 vs 직교", English: "PROJECTION", Minutes: 5,
                    Pages:
                    [
                        new(
                            "PerspectiveCamera ↔ OrthographicCamera",
                            [
                                "원근 투영은 멀수록 작게 — 사람 눈과 같아요. 직교 투영은 거리와 무관하게 같은 크기 — 도면·CAD·아이소메트릭 게임 뷰가 이거예요. 길이를 비교하거나 정확히 정렬할 때는 직교가 훨씬 편해요.",
                                "FieldOfView(원근의 화각)를 키우면 광각처럼 왜곡되고, NearPlaneDistance/FarPlaneDistance 밖의 물체는 잘려요. \"가까이 갔더니 물체가 뚫린다\"는 대부분 Near 값이 큰 탓입니다.",
                            ],
                            """
                            // 원근: 멀수록 작게
                            new PerspectiveCamera { FieldOfView = 50, NearPlaneDistance = 0.1 };

                            // 직교: Width가 "화면에 담을 월드 폭"
                            new OrthographicCamera { Width = 10 };
                            """),
                    ]),
                new(
                    Id: "a7", Kind: LessonKind.Reading, Title: "3D 애니메이션", English: "3D ANIMATION", Minutes: 5,
                    Pages:
                    [
                        new(
                            "Storyboard로 눈사람 돌리기",
                            [
                                "WPF 3D의 애니메이션은 2D와 완전히 같은 시스템이에요. Rotation3DAnimation·Vector3DAnimation·Point3DAnimation을 Storyboard에 담아 Transform의 속성을 시간에 따라 바꿉니다.",
                                "버튼 색 애니메이션을 만들 줄 알면 3D 회전 애니메이션도 이미 아는 것 — 대상 속성 경로만 3D 타입일 뿐이에요.",
                            ],
                            """
                            var anim = new Rotation3DAnimation(
                                new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0),
                                new AxisAngleRotation3D(new Vector3D(0, 1, 0), 360),
                                TimeSpan.FromSeconds(6)) { RepeatBehavior = RepeatBehavior.Forever };

                            rotateTransform.BeginAnimation(RotateTransform3D.RotationProperty, anim);
                            """),
                    ]),
                new(
                    Id: "a8", Kind: LessonKind.Reading, Title: "3D 히트 테스트 — 클릭 선택의 원리", English: "PICKING", Minutes: 5,
                    Pages:
                    [
                        new(
                            "VisualTreeHelper.HitTest",
                            [
                                "마우스 위치에서 장면 안으로 광선을 쏘아 처음 맞는 삼각형을 찾는 것이 히트 테스트(피킹)예요. 폴리랩에서 오브젝트를 클릭해 선택할 때 내부적으로 일어나는 일이 바로 이것입니다.",
                                "WPF에서는 VisualTreeHelper.HitTest에 Viewport3D와 2D 좌표를 넘기면 RayMeshGeometry3DHitTestResult로 맞은 모델·삼각형 인덱스·정확한 3D 좌표까지 알려줘요.",
                            ],
                            """
                            void OnClick(object s, MouseButtonEventArgs e)
                            {
                                var pt = e.GetPosition(viewport);
                                var hit = VisualTreeHelper.HitTest(viewport, pt)
                                          as RayMeshGeometry3DHitTestResult;
                                if (hit != null)
                                {
                                    var model = hit.ModelHit;        // 맞은 모델
                                    var where = hit.PointHit;        // 정확한 3D 좌표
                                }
                            }
                            """),
                    ]),
                new(
                    Id: "a9", Kind: LessonKind.Reading, Title: "순수 WPF의 한계 — 정직하게", English: "THE HONEST CEILING", Minutes: 5,
                    Pages:
                    [
                        new(
                            "순정 Media3D가 못 하는 것",
                            [
                                "① 그림자 매핑 없음 — 진짜 그림자는 못 만들어요. ② 조명 모델이 Phong 계열 — 현대적 PBR(물리 기반 재질)이 아니에요. ③ 셰이더를 직접 쓸 수 없어요 — 렌더링 파이프라인이 밀봉되어 있어요. ④ 수십만 폴리곤·대량 오브젝트에 약해요.",
                                "이건 결함이 아니라 설계예요. WPF 3D는 \"UI에 가벼운 3D를 곁들이는\" 용도로 만들어졌어요. 진짜 그림자·PBR·성능이 필요해지는 순간이 트랙 B로 넘어갈 때입니다.",
                                "참고: Microsoft Learn의 3-D Graphics Overview / Maximize WPF 3D Performance 문서가 이 한계를 공식적으로 설명해요.",
                            ]),
                    ]),
                new(
                    Id: "q", Kind: LessonKind.Quiz, Title: "개념 체크: 트랙 A 종합", English: "QUIZ", Minutes: 4,
                    Questions:
                    [
                        new("TriangleIndices를 반시계 방향으로 적는 이유는?", ["면의 앞면(와인딩)을 정하기 위해", "메모리를 아끼려고", "알파벳 순서라서"], 0,
                            "적는 순서가 앞면을 결정해요. 반대로 적으면 후면 컬링으로 면이 사라져 보여요."),
                        new("회전 애니메이션 중 축이 겹쳐 이상하게 도는 현상과 해결책은?", ["짐벌락 — QuaternionRotation3D로 해결", "Z-파이팅 — 카메라를 옮겨 해결", "오버드로 — 컬링으로 해결"], 0,
                            "오일러 각의 숙명인 짐벌락은 쿼터니언 회전으로 피할 수 있어요."),
                        new("조명을 수십 개로 늘리면 WPF 3D에서 무슨 일이 생기나요?", ["자동으로 그림자가 생긴다", "소프트웨어 렌더링으로 후퇴해 급격히 느려진다", "아무 일도 없다"], 1,
                            "대략 Directional 110 / Point 70 / Spot 40개 근처가 하드웨어 렌더링의 절벽이에요."),
                        new("3D 오브젝트 클릭 선택(피킹)에 쓰는 WPF API는?", ["VisualTreeHelper.HitTest", "Mouse.Capture", "Raycaster3D"], 0,
                            "Viewport3D와 2D 좌표를 넘기면 RayMeshGeometry3DHitTestResult로 맞은 모델을 알려줘요."),
                    ]),
            ]),
        new(
            Id: "tb", Num: "B", Title: "트랙 B · Windows용 .NET 3D 도구", English: "DIRECTX-BASED .NET 3D (WINDOWS ONLY)",
            Description: "순정의 한계(A-9)에 부딪혔을 때 올라가는 DirectX 사다리예요. 각 도구를 \"언제 쓰나 / 무엇을 얻나 / 무엇을 포기하나\"로 배웁니다. 전부 Windows 전용입니다.",
            Lessons:
            [
                new(
                    Id: "b1", Kind: LessonKind.Reading, Title: "HelixToolkit.Wpf — 첫 번째 계단", English: "CONVENIENCE ON TOP OF MEDIA3D", Minutes: 6,
                    Pages:
                    [
                        new(
                            "순정 위에 얹은 편의 기능",
                            [
                                "HelixToolkit.Wpf는 새 엔진이 아니라, 지금까지 배운 Media3D 위에 편의 기능을 얹은 라이브러리예요. 내부 렌더링은 여전히 WPF 순정 — 배운 개념이 100% 그대로 통합니다.",
                                "얻는 것: HelixViewport3D(궤도·줌·팬 카메라 내장 — 우리가 직접 짰던 UpdateCamera가 공짜), MeshBuilder(구·상자·튜브를 한 줄로), 그리고 순정에 없는 ModelImporter로 OBJ/STL/3DS 파일 임포트. 좌표축·바운딩박스 헬퍼도 있어요.",
                            ],
                            """
                            <!-- 카메라 조작이 내장된 뷰포트 -->
                            <helix:HelixViewport3D ZoomExtentsWhenLoaded="True">
                                <helix:SunLight/>
                                <helix:GridLinesVisual3D/>
                            </helix:HelixViewport3D>

                            // Blender에서 내보낸 모델 불러오기 (순정엔 없는 기능!)
                            var model = new ModelImporter().Load("snowman.obj");
                            """),
                        new(
                            "한계는 그대로",
                            [
                                "순정 위에 얹은 것이라 그림자 없음·PBR 아님 같은 근본 한계는 똑같아요. 좌표계도 WPF와 같은 오른손 좌표계.",
                                "판단 기준: \"카메라·임포터를 직접 짜기 귀찮다\"면 여기까지로 충분. \"그림자와 성능\"이 필요하면 다음 계단으로.",
                            ]),
                    ]),
                new(
                    Id: "b2", Kind: LessonKind.Reading, Title: "HelixToolkit.SharpDX — DX11 엔진", English: "DIRECTX 11 ENGINE IN WPF", Minutes: 6,
                    Pages:
                    [
                        new(
                            "순정을 버리고 DX11로",
                            [
                                "HelixToolkit.Wpf.SharpDX는 Media3D를 버리고 SharpDX(DirectX 11) 기반 커스텀 엔진으로 그려서 WPF 창 안에 삽입해요.",
                                "얻는 것: 진짜 그림자, 실제 PBR 재질, FXAA·순서독립투명(OIT)·파티클·테셀레이션, 그리고 대량 폴리곤 성능. A-9에서 본 한계를 대부분 돌파합니다. 같은 눈사람 장면을 순정과 나란히 렌더해 보면 그림자·반사 차이가 한눈에 보여요.",
                            ]),
                        new(
                            "결정적 제약 — 여기까지가 DX11의 세계",
                            [
                                "이 엔진은 DirectX 11 전용이에요. SharpDX 자체가 2019년 개발이 중단됐고, HelixToolkit이 내부에서 DX11용으로 계속 쓰고 있을 뿐입니다. (HelixToolkit v2는 유지보수 모드, v3로 이행 중.)",
                                "따라서 DX12·DXR(하드웨어 레이트레이싱)·메시 셰이더 같은 최신 기능은 이 경로로는 못 씁니다. DX12가 필요하면 다음 레슨의 브릿지를 직접 놓아야 해요.",
                            ]),
                    ]),
                new(
                    Id: "b3", Kind: LessonKind.Reading, Title: "DX11 vs DX12 — WPF에서 DX12를 쓰려면", English: "THE DX12 BRIDGE", Minutes: 10,
                    Pages:
                    [
                        new(
                            "근본 문제: WPF의 표면은 우리가 못 만진다",
                            [
                                "WPF의 합성 표면은 렌더 스레드(milcore)가 독점해요. 외부 DirectX 내용을 받는 유일한 공식 창구는 D3DImage인데, 이건 D3D9 표면만 받습니다.",
                                "그래서 DX11/DX12 결과를 WPF에 보여주려면 공유 표면(shared surface)으로 다리를 놓아야 해요. 접근은 두 가지이고, 트레이드오프가 정반대입니다.",
                            ]),
                        new(
                            "접근 1 — D3DImage 공유 표면 브릿지 (WPF와 합성됨)",
                            [
                                "흐름: DX12로 렌더 → 공유 DXGI 리소스 → D3D11On12로 D3D11 텍스처로 감쌈 → D3D11Image(microsoft/WPFDXInterop)가 내부에서 D3D9로 브릿지 → WPF에 표시.",
                                "장점: WPF 버튼·패널을 3D 위에 겹칠 수 있고 레이아웃에 자연스럽게 녹아들어요. 단점: 매 프레임 복사가 일어나고 WPF 렌더 루프에 묶여 성능 손해, 배선이 복잡해요.",
                            ],
                            """
                            // 핵심 부품
                            D3D11On12CreateDevice(dx12Device, ...);  // DX12 ↔ DX11 상호운용
                            // microsoft/WPFDXInterop의 D3D11Image가
                            // D3D11 텍스처 → D3DImage(D3D9) 브릿지를 담당
                            <Image Source="{Binding D3D11Image}"/>  // WPF에 합성
                            """),
                        new(
                            "접근 2 — HwndHost 에어스페이스 (DX12 본연의 성능)",
                            [
                                "자식 HWND를 만들고 그 위에 DXGI 스왑체인을 얹어 DX12가 직접 렌더·프레젠트해요. 복사가 없어 DX12 성능을 그대로 얻고, 자체 present 루프를 가질 수 있어요.",
                                "대가는 에어스페이스 문제 — 그 영역 위에 WPF 요소를 얹을 수 없고, 마우스/키보드 이벤트를 수동으로 전달해야 하며, AllowsTransparency 창과 충돌해요.",
                                "바인딩 선택: SharpDX는 중단됐으니 Vortice.Windows(D3D11·D3D12·DXGI·DXR·D3D11On12 모두 지원, .NET 9/10 타깃, Vortice.Wpf 헬퍼와 HelloDirect3D12 샘플 제공)가 표준 선택이에요. Silk.NET도 D3D12를 포함하지만 WPF 예제는 Vortice가 더 직접적입니다.",
                                "정직한 판단: 대부분의 WPF 3D(LOB·시각화·CAD성 뷰어)에는 DX11로 충분해요. 어차피 D3DImage 복사가 병목이면 DX12의 CPU 이점이 상쇄됩니다. DX12가 값어치를 하는 경우는 DXR 레이트레이싱, 메시 셰이더, 초대량 드로우콜, 기존 DX12 렌더러 공유 — 이럴 때만 브릿지 비용을 지불하세요.",
                            ]),
                    ]),
                new(
                    Id: "b4", Kind: LessonKind.Reading, Title: "저수준과 엔진 — 더 멀리", English: "LOW-LEVEL & ENGINES", Minutes: 5,
                    Pages:
                    [
                        new(
                            "Vortice/Silk.NET 직접 제어, 그리고 Stride",
                            [
                                "셰이더·버퍼·파이프라인 스테이트를 손수 설정하는 저수준 제어는 엔진을 직접 만들거나 최신 GPU 기능을 쓸 때의 길이에요. B-3에서 놓은 브릿지의 연장선입니다.",
                                "Stride는 .NET Foundation의 오픈소스 3D 게임 엔진(씬 에디터·PBR·물리 완비)이에요. 다만 이건 WPF에 삽입하는 게 아니라 독립 엔진 — \"3D가 앱의 일부\"가 아니라 \"경험 전체\"일 때의 선택지입니다. 실제로 설치하고 Blender에서 만든 모델을 올려 C#으로 움직여 보는 것은 트랙 C에서 다뤄요.",
                                "학습 사다리 정리: ① 순정 Media3D로 원리 → ② HelixToolkit.Wpf로 편의 → ③ HelixToolkit.SharpDX로 품질·성능(DX11 천장) → ④ Vortice/Silk.NET로 DX12·최대 제어 → ⑤ Stride로 엔진 위에서. 계단마다 같은 눈사람 장면을 다시 만들어 보세요 — 도구가 바뀌어도 개념은 그대로임을 체득하게 됩니다.",
                            ]),
                    ]),
                new(
                    Id: "q", Kind: LessonKind.Quiz, Title: "개념 체크: 트랙 B 종합", English: "QUIZ", Minutes: 4,
                    Questions:
                    [
                        new("WPF가 외부 DirectX 내용을 받는 유일한 공식 창구 D3DImage가 수용하는 표면은?", ["D3D9 표면만", "D3D11 표면만", "D3D12 표면만"], 0,
                            "그래서 DX11/DX12 결과는 공유 표면 브릿지(D3D11On12 → D3D11Image → D3D9)를 거쳐야 해요."),
                        new("HelixToolkit.Wpf.SharpDX 엔진이 묶여 있는 DirectX 버전은?", ["DX11", "DX12", "DX9"], 0,
                            "SharpDX(2019년 중단) 기반 DX11 전용이라 DXR·메시 셰이더는 이 경로로 못 써요."),
                        new("SharpDX의 현대적 후계로 DX12·DXR을 지원하는 .NET 바인딩은?", ["Vortice.Windows", "XNA", "Managed DirectX"], 0,
                            "D3D11·D3D12·DXGI·DXR·D3D11On12를 모두 지원하고 활발히 유지보수돼요. Silk.NET도 대안입니다."),
                        new("HwndHost 방식으로 DX12를 띄울 때의 대표적 제약은?", ["에어스페이스 — 그 위에 WPF 요소를 못 얹음", "OBJ 임포트가 안 됨", "카메라를 쓸 수 없음"], 0,
                            "DX12가 자기 HWND에 직접 그리는 대신, WPF와의 합성을 포기하는 트레이드오프예요."),
                    ]),
            ]),
        new(
            Id: "tc", Num: "C", Title: "트랙 C · Blender에서 Stride까지", English: "THE BLENDER-TO-STRIDE PIPELINE",
            Description: "트랙 B 사다리의 마지막 칸(⑤ Stride)을 실제로 걸어 보는 트랙이에요. 파이썬으로 모델을 만들고(Blender), 파일로 내보내고(FBX·GLB), 엔진에서 C#으로 제어합니다(Stride). 폴리랩 밖의 도구를 쓰는 유일한 트랙이니, 설치가 안 되어 있다면 코스 00을 먼저 보세요.",
            Lessons:
            [
                new(
                    Id: "c1", Kind: LessonKind.Reading, Title: "Blender Scripting — 파이썬으로 모델 띄우기", English: "BUILDING A MESH WITH BPY", Minutes: 10,
                    Pages:
                    [
                        new(
                            "Scripting 워크스페이스",
                            [
                                "Blender 상단 탭의 맨 오른쪽 Scripting을 누르면 파이썬 콘솔·텍스트 에디터·Info 로그가 한 화면에 나와요. 에디터에 코드를 쓰고 Alt+P로 실행하면 결과가 그 즉시 3D 뷰포트에 나타납니다.",
                                "핵심은 이겁니다 — Blender에서 마우스로 하는 모든 조작은 bpy API 호출과 1:1로 대응해요. Info 로그에 방금 한 조작의 파이썬 코드가 그대로 찍히니, 손으로 한 번 해 보고 그 코드를 복사하는 것이 가장 빠른 학습법입니다.",
                            ]),
                        new(
                            "폴리랩의 MeshFactory와 똑같은 일",
                            [
                                "from_pydata(정점 목록, 엣지 목록, 면 인덱스 목록)은 정점 배열과 면 인덱스를 그대로 넘겨 메시를 만드는 함수예요. 폴리랩의 MeshFactory가 Point3를 쌓고 AddTriangle로 인덱스를 적는 것과 완전히 같은 일을 합니다.",
                                "도구가 Blender든 WPF든, 메시의 정의는 정점 + 인덱스라는 사실이 변하지 않아요. 트랙 A에서 배운 와인딩 순서도 여기서 그대로 통합니다 — 면 인덱스를 반대로 적으면 노멀이 뒤집혀요.",
                                "objects.link(obj)를 호출하는 순간 오브젝트가 장면에 들어가 뷰포트에 보입니다. 그 앞까지는 데이터만 존재하고 화면에는 없어요 — 폴리랩에서 MeshGeometry3D를 만들어도 ModelVisual3D로 Viewport3D에 넣기 전까지 안 보이는 것과 같은 구조입니다.",
                            ],
                            """
                            import bpy

                            bpy.ops.wm.read_factory_settings(use_empty=True)   # 빈 장면에서 시작

                            # 정점과 면 인덱스를 직접 넘겨 메시를 만든다
                            mesh = bpy.data.meshes.new("TowerMesh")
                            verts = [(-.5, -.5, 0), (.5, -.5, 0), (.5, .5, 0), (-.5, .5, 0),
                                     (-.3, -.3, 1.4), (.3, -.3, 1.4), (.3, .3, 1.4), (-.3, .3, 1.4)]
                            faces = [(0, 1, 2, 3), (7, 6, 5, 4), (0, 4, 5, 1),
                                     (1, 5, 6, 2), (2, 6, 7, 3), (3, 7, 4, 0)]
                            mesh.from_pydata(verts, [], faces)
                            mesh.update()

                            obj = bpy.data.objects.new("Tower", mesh)
                            bpy.context.collection.objects.link(obj)   # 이 순간 뷰포트에 나타난다

                            # 모디파이어와 변형도 전부 스크립트로
                            obj.modifiers.new(name="Bevel", type="BEVEL").width = 0.03
                            obj.rotation_euler = (0, 0, 0.6)
                            """),
                    ]),
                new(
                    Id: "c2", Kind: LessonKind.Reading, Title: "내보내기 — FBX와 GLB 사이에서", English: "EXPORTING THE MODEL", Minutes: 8,
                    Pages:
                    [
                        new(
                            "내보내기도 한 줄이에요",
                            [
                                "메뉴의 File > Export에 있는 항목들은 모두 오퍼레이터 호출이라, 스크립트에서 그대로 부를 수 있어요. FBX와 glTF는 export_scene 계열이고, OBJ·STL·PLY는 wm 계열의 새 이름을 씁니다.",
                                "재미있는 실험 하나 — 앞 레슨에서 만든 정점 8개짜리 상자를 두 형식으로 내보내면 FBX는 약 11,800바이트, GLB는 약 1,500바이트가 나옵니다. 같은 모양인데 8배 차이예요. FBX가 담는 부가 정보가 그만큼 많고 형식이 무겁다는 뜻입니다.",
                            ],
                            """
                            # FBX — 스켈레톤·애니메이션까지 확실하게
                            bpy.ops.export_scene.fbx(filepath=r"C:\out\tower.fbx", use_selection=False)

                            # GLB — 열린 규격, PBR 재질이 규격에 내장
                            bpy.ops.export_scene.gltf(filepath=r"C:\out\tower.glb", export_format="GLB")

                            # 그 밖에
                            bpy.ops.wm.obj_export(filepath=r"C:\out\tower.obj")
                            bpy.ops.wm.stl_export(filepath=r"C:\out\tower.stl")

                            # 같은 8정점 상자 결과: FBX 11,820 bytes / GLB 1,484 bytes
                            """),
                        new(
                            "축과 단위 — 모델이 누워서 들어오는 이유",
                            [
                                "Blender는 Z가 위인 좌표계인데 FBX와 glTF는 Y가 위인 것이 관례예요. 그래서 내보내기 단계에서 축 변환이 일어납니다. 엔진에서 모델이 옆으로 누워 있거나 90도 돌아가 있으면, 재질이나 메시가 아니라 이 업축 변환부터 확인하세요.",
                                "단위도 같은 함정이에요. Blender의 1은 기본적으로 1미터인데 형식과 도구마다 해석이 달라서, 모델이 1000배 크거나 작게 들어오는 일이 흔합니다.",
                                "무엇을 고를까: 캐릭터 스켈레톤과 애니메이션을 함께 넘겨야 하면 FBX가 여전히 가장 확실합니다. 정적 모델과 PBR 재질이 목적이면 GLB가 더 가볍고, 규격이 공개되어 있어 도구 간 결과가 일관돼요. Stride는 둘 다 읽으니 강제되는 선택은 없습니다.",
                            ]),
                    ]),
                new(
                    Id: "c3", Kind: LessonKind.Reading, Title: "Stride — 가져오기와 C# 제어", English: "IMPORT AND CONTROL IN STRIDE", Minutes: 10,
                    Pages:
                    [
                        new(
                            "끌어다 놓으면 에셋이 된다",
                            [
                                "Game Studio의 Asset View에 파일을 끌어다 놓으면 Stride의 에셋 파이프라인이 모델·재질·텍스처 에셋으로 컴파일해요. 원본 파일을 매번 파싱하는 게 아니라, 엔진이 바로 올릴 수 있는 형태로 미리 굽는(bake) 구조입니다.",
                                "Stride 4.3의 모델 임포터는 Assimp 기반 통합 임포터(Stride.Importer.3D)라서 예전처럼 Autodesk FBX SDK를 따로 설치하지 않아도 돼요. 읽는 확장자 목록에 .blend가 직접 들어 있는 것도 눈에 띕니다 — 급할 때는 내보내기 없이 .blend를 바로 던져 볼 수 있어요.",
                            ],
                            """
                            Stride 4.3 모델 임포터가 읽는 확장자
                            (Stride.Assets.Models 어셈블리에 선언된 목록)

                            .dae  .3ds  .gltf  .glb  .obj  .blend  .x
                            .md2  .md3  .dxf   .ply  .stl  .stp    .fbx
                            """),
                        new(
                            "SyncScript — 매 프레임 불리는 Update()",
                            [
                                "Stride에서 오브젝트를 제어하는 기본 방법은 SyncScript를 상속한 클래스를 만들고 Update()를 채우는 거예요. 그 스크립트를 Game Studio에서 엔티티에 컴포넌트로 붙이면, 매 프레임 Update()가 호출됩니다.",
                                "구조가 익숙할 겁니다 — Entity.Transform.Rotation에 회전을 곱해 쌓는 것은 폴리랩에서 Transform3DGroup에 RotateTransform3D를 넣는 것과 같은 개념이고, 짐벌락을 피하려 Quaternion을 쓰는 이유도 트랙 A에서 배운 그대로예요.",
                                "다른 점은 시간입니다. Game.UpdateTime.Elapsed로 지난 프레임의 경과 시간(델타 타임)을 받아 곱해 줘야, 프레임 레이트가 달라도 같은 속도로 움직여요. 이걸 빼먹으면 빠른 PC에서 모델이 미친 듯이 돕니다.",
                            ],
                            """
                            using Stride.Core.Mathematics;
                            using Stride.Engine;
                            using Stride.Input;

                            public class SpinModel : SyncScript   // 매 프레임 Update()가 불린다
                            {
                                public float Speed { get; set; } = 1.0f;

                                public override void Update()
                                {
                                    float dt = (float)Game.UpdateTime.Elapsed.TotalSeconds;

                                    // 델타 타임을 곱해야 프레임 레이트와 무관하게 같은 속도
                                    Entity.Transform.Rotation *= Quaternion.RotationY(Speed * dt);

                                    if (Input.IsKeyDown(Keys.Right))
                                    {
                                        Entity.Transform.Position += new Vector3(2f * dt, 0f, 0f);
                                    }

                                    DebugText.Print("pos = " + Entity.Transform.Position, new Int2(20, 20));
                                }
                            }
                            """),
                        new(
                            "그래서 언제 이 길을 가나",
                            [
                                "판단 기준은 트랙 B와 같아요. 3D가 앱의 한 부분이면(대시보드 안의 3D 뷰, 설비 시각화, 뷰어) WPF에 남는 것이 낫습니다. 3D가 경험의 전체라면 엔진으로 올라오세요.",
                                "폴리랩 자신은 이 파일들을 읽지 못합니다. 순정 WPF에는 임포터가 없고, 폴리랩의 장면은 코드로 생성한 프리미티브(ShapeKind)로만 만들어져 있어요. 그건 결함이 아니라 역할 분담입니다 — 폴리랩은 개념을 손으로 만져 보는 실험실이고, Stride는 완성품을 올리는 무대예요.",
                                "이 트랙을 한 문장으로: 파이썬으로 만들고(Blender) → 파일로 굳히고(FBX·GLB) → C#으로 움직인다(Stride). 세 단계 모두에서 다루는 것은 결국 정점·인덱스·변형·재질 — 트랙 A에서 순정 API로 이미 겪은 그 개념들입니다.",
                            ]),
                    ]),
                new(
                    Id: "q", Kind: LessonKind.Quiz, Title: "개념 체크: 트랙 C 종합", English: "QUIZ", Minutes: 4,
                    Questions:
                    [
                        new("Blender에서 Stride로 넘길 때 FBX 대신 GLB를 고르면 얻는 것은?", ["열린 규격이라 도구 간 결과가 일관되고 파일이 가볍다", "애니메이션을 더 많이 담을 수 있다", "Stride가 GLB만 읽는다"], 0,
                            "스켈레톤 애니메이션이 필요하면 FBX가 확실하고, 정적 모델과 PBR 재질이면 GLB가 가볍고 일관돼요. Stride는 둘 다 읽습니다."),
                        new("Blender에서 정점 배열과 면 인덱스를 직접 넘겨 메시를 만드는 함수는?", ["mesh.from_pydata", "bpy.ops.mesh.build", "mesh.load_vertices"], 0,
                            "폴리랩 MeshFactory가 Point3를 쌓고 AddTriangle을 부르는 것과 같은 일을 한 번에 합니다."),
                        new("Stride 4.3의 모델 임포터가 Assimp 기반으로 통합되면서 달라진 점은?", ["FBX SDK를 따로 설치하지 않아도 FBX·GLB·OBJ 등을 함께 읽는다", "FBX만 읽게 되었다", "임포트 기능이 없어졌다"], 0,
                            "Stride.Importer.3D 하나가 .fbx·.gltf·.glb·.obj·.ply·.blend 등을 모두 담당해요."),
                        new("Blender에서 내보낸 모델이 엔진에서 누워 있을 때 가장 먼저 볼 곳은?", ["업축 변환 (Blender Z-up ↔ FBX·glTF Y-up)", "재질의 거칠기 값", "카메라의 화각"], 0,
                            "축 관례가 도구마다 달라서 생기는 가장 흔한 사고예요. 단위 배율도 같은 부류의 함정입니다."),
                        new("Stride의 Update()에서 델타 타임(Game.UpdateTime.Elapsed)을 곱하지 않으면?", ["프레임 레이트가 높은 PC에서 훨씬 빠르게 움직인다", "아무 차이가 없다", "컴파일되지 않는다"], 0,
                            "프레임당 같은 양을 더하므로 초당 프레임 수만큼 속도가 달라져요. 시간 기반으로 움직여야 합니다."),
                    ]),
            ]),
    ];
}
