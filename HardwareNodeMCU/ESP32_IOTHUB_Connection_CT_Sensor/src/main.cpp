#include <WiFi.h>
#include "AzureIotHub.h"
#include "Esp32MQTTClient.h"
#include "EmonLib.h"

#define INTERVAL 1000
#define DEVICE_ID "CT_Sensor2"
#define MESSAGE_MAX_LEN 256
#define ADC_INPUT 34
#define HOME_VOLTAGE 220.0

// Create instances
EnergyMonitor emon1;

// SSID and password of WiFi
const char* ssid     = "XXXXXXX";
const char* password = "XXXXXXXXX";

static const char* connectionString = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

const char *messageData = "{\"locationId\":\"%d\",\"deviceId\":\"%s\", \"reportedAt\":%s,\"Power\":%f, \"Temperature\":%f, \"Humidity\":%f, \"Occupancy\":%d, \"ArmState\":%s}";

double watt = 0.0;
double amps = 0.0;
int messageCount = 1;
static bool hasWifi = false;
static bool messageSending = true;
static uint64_t send_interval_ms;
unsigned long lastMeasurement = 0;

//////////////////////////////////////////////////////////////////////////////////////////////////////////
// Utilities
static void InitWifi()
{
  Serial.println("Connecting...");
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  hasWifi = true;
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
}

static void SendConfirmationCallback(IOTHUB_CLIENT_CONFIRMATION_RESULT result)
{
  if (result == IOTHUB_CLIENT_CONFIRMATION_OK)
  {
    Serial.println("Send Confirmation Callback finished.");
  }
}

static void MessageCallback(const char* payLoad, int size)
{
  Serial.println("Message callback:");
  Serial.println(payLoad);
}

static void DeviceTwinCallback(DEVICE_TWIN_UPDATE_STATE updateState, const unsigned char *payLoad, int size)
{
  char *temp = (char *)malloc(size + 1);
  if (temp == NULL)
  {
    return;
  }
  memcpy(temp, payLoad, size);
  temp[size] = '\0';
  // Display Twin message.
  Serial.println(temp);
  free(temp);
}

static int  DeviceMethodCallback(const char *methodName, const unsigned char *payload, int size, unsigned char **response, int *response_size)
{
  LogInfo("Try to invoke method %s", methodName);
  const char *responseMessage = "\"Successfully invoke device method\"";
  int result = 200;

  if (strcmp(methodName, "start") == 0)
  {
    LogInfo("Start sending temperature and humidity data");
    messageSending = true;
  }
  else if (strcmp(methodName, "stop") == 0)
  {
    LogInfo("Stop sending temperature and humidity data");
    messageSending = false;
  }
  else
  {
    LogInfo("No method %s found", methodName);
    responseMessage = "\"No method found\"";
    result = 404;
  }

  *response_size = strlen(responseMessage) + 1;
  *response = (unsigned char *)strdup(responseMessage);

  return result;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////
// Arduino sketch
void setup()
{
  Serial.begin(115200);
  Serial.println("ESP32 Device");
  Serial.println("Initializing...");
  
  // Initialize emon library (30 = calibration number)
  emon1.current(ADC_INPUT, 1);
  // Initialize the WiFi module
  Serial.println(" > WiFi");
  hasWifi = false;
  InitWifi();
  if (!hasWifi)
  {
    return;
  }
  randomSeed(analogRead(34));

  Serial.println(" > IoT Hub");
  Esp32MQTTClient_SetOption(OPTION_MINI_SOLUTION_NAME, "GetStarted");
  Esp32MQTTClient_Init((const uint8_t*)connectionString, true);

  Esp32MQTTClient_SetSendConfirmationCallback(SendConfirmationCallback);
  Esp32MQTTClient_SetMessageCallback(MessageCallback);
  Esp32MQTTClient_SetDeviceTwinCallback(DeviceTwinCallback);
  Esp32MQTTClient_SetDeviceMethodCallback(DeviceMethodCallback);

  send_interval_ms = millis();
}

void loop()
{
    
  if (hasWifi)
  {
    if (messageSending && (int)(millis() - send_interval_ms) >= INTERVAL)
    {   
      watt = 0;   
      amps = 0;
      amps = emon1.calcIrms(1480); // Calculate Irms only 
      watt = (amps * HOME_VOLTAGE); 
      Serial.println(watt);

      // Send teperature data
      char messagePayload[MESSAGE_MAX_LEN];
      float temperature = (float)random(0,50);
      float humidity = (float)random(0, 1000)/10;
      //float power = (float)random(0, 1000);
      int occupancy = (int)random(0, 1);
      //get UTC Time
      time_t ttime = time(0);    
      char* dt = ctime(&ttime);
      tm *gmt_time = gmtime(&ttime);
      dt = asctime(gmt_time);

      snprintf(messagePayload,MESSAGE_MAX_LEN, messageData,1234,DEVICE_ID, dt,watt,temperature,humidity,occupancy,"ArmAway");
      Serial.println(watt);
      EVENT_INSTANCE* message = Esp32MQTTClient_Event_Generate(messagePayload, MESSAGE);
     // Esp32MQTTClient_Event_AddProp(message, "temperatureAlert", "true");
      Esp32MQTTClient_SendEventInstance(message);
      
      send_interval_ms = millis();
    }
    else
    {
      Esp32MQTTClient_Check();
    }
  }
  delay(10);
}
