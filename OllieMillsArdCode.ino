#include <SPI.h>
#include <MFRC522.h>
 
#define SS_PIN 10
#define RST_PIN 9
MFRC522 mfrc522(SS_PIN, RST_PIN);

String responce = "";

void setup() 
{
  pinMode(7, OUTPUT);//Green
  pinMode(6, OUTPUT);//Red
  pinMode(5, OUTPUT);//Blue
  Serial.begin(9600);   
  SPI.begin();          
  mfrc522.PCD_Init();   
}

void loop() 
{
  if (!mfrc522.PICC_IsNewCardPresent()) 
  {
    return;
  }
  if (!mfrc522.PICC_ReadCardSerial()) 
  {
    return;
  }
  
  Serial.print("ARD 2");
  String content = "";
  for (byte i = 0; i < mfrc522.uid.size; i++) 
  {
     Serial.print(mfrc522.uid.uidByte[i] < 0x10 ? " 0" : " ");
     Serial.print(mfrc522.uid.uidByte[i], HEX);
     content.concat(String(mfrc522.uid.uidByte[i] < 0x10 ? " 0" : " "));
     content.concat(String(mfrc522.uid.uidByte[i], HEX));
  }
  Serial.println();  
  
  delay(500);    

  if (Serial.available() > 0) {
    responce = Serial.readString();  
    responce.trim();                 
  }
  if (responce == "True") // green
  {
    flickerLights(7);
  }
  else if (responce == "False") // red
  {
    flickerLights(6);
  }
  else // blue
  {
    flickerLights(5);
  }
}
void flickerLights(int pin) {
  for (int i = 0; i < 3; i++) {
    digitalWrite(pin, HIGH);
    delay(200);
    digitalWrite(pin, LOW);
    delay(200);
  }
  responce = "";
}
