E�� ���� ��������� �� ������ ������� ����. ��� 5 ����� ���� �������� � ������ � ������� ������ "UNITY_INITIALIZE_OUTPUT(Input,o);"

��� ���:

void vert (inout appdata_full v, out Input o) {
UNITY_INITIALIZE_OUTPUT(Input,o);
float4 oPos = mul(UNITY_MATRIX_MVP, v.vertex);

���� ��������� �����-���� �������, ������ ������ � ����� "scar289"