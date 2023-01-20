# DsFunctions

* DamulSoft Functions

## Nuget Package 만드는 방법 (잊어먹어서 여기에 적자)

* 빌드 > 팩 DsFunction 클릭하면 nupkg 파일을 생성한다. 
* 이 파일을 Nuget 사이트에 가서 업로드한다. 

## Components Class

### DoubleBufferTableLayoutPanel

* 폼에 콘트롤을 구성할 때 TableLayoutPanel을 많이 사용합니다. 그런데 이 판넬을 사용하고 폼을 이동하면 깜빡이는 현상이 발생합니다. 
* 이 문제를 해결하기 위해 DoubleBuffer를 사용하는 클래스입니다. 
* 사용방법
  * TableLayoutPanel을 사용하는 디자이너 소스에서 DoubleBufferTableLayoutPanel으로 모두 변경해주시면 됩니다. 

## Common Class

### DsDatetime

* 날짜/시간 관련 Helper Class

### DsFont

* 폰트 관련 클래스 

### DsVersion

* 버전 관련 클래스

### DsNetwork

* 네트워크 관련 클래스

### DsLogEvent

* 윈도우 이벤트 로그에 로그를 발생하는 클래스

### DsFile

* 파일, 폴더관련 클래스

### DsEnumHelper 

* Enum Helper 클래스

## Security

### DsCrypto

* 암/복호화 관련 함수 (Aria, AES, Base64 Encoder/Decode, Sha256 Hash)