Eщё хочу дополнить по поводу шейдера льда. Для 5 юнити надо добавить в шейдер с ошибкой строку "UNITY_INITIALIZE_OUTPUT(Input,o);"

Вот так:

void vert (inout appdata_full v, out Input o) {
UNITY_INITIALIZE_OUTPUT(Input,o);
float4 oPos = mul(UNITY_MATRIX_MVP, v.vertex);

Если возникнут какие-либо вопросы, можете писать в скайп "scar289"