<h3 align="center">SelfieSmarTSensorZ - Super Saver Self sustained Energy Efficient in-Home Intelligence   </h3>

<div align="center">

</div>

---
## Problem statement

* Electricity usage has increased exponentially due to large usage of appliances. Due to this level of usage, the increase in the electricity bill is inevitable.
* Many units of power are being wasted due to negligent usage in residential/Commercial like appliance keep on running if people not around.
* Manually monitoring the electricity usage for the appliances will be hectic and will go out for maintenance in case of repair or faulted
* we have lot of smart home sensors like PIR,Motion,air quality, Occupancy sensor etc. that solves only for specific problem for homeowner and need to monitor these sensor events and take actions individually
* we don't have system today to collect/learn the all-available Home sensors data combined to detect devices, devices usage and device performance on long run to reduce power and maintenance at right time
* The homeowner expects single comprehensive self-monitoring home intelligently with the available sensors it means the Home speaks to homeowner that will give care/comfort and save money 
* Most of this wastage can be ceased by proper monitoring of usage. Understanding how much energy their home is using, when and where, empowers them to find savings
* Sensor devices like Smappee and Sense are relatively expensive which don’t used combined sensors data and even require a subscription.
 
## How it helps to solve the problem?

* The proposed approach used the different sensors combination to detect devices, usage ,performance, person occupancy and temperature at home. 
* The smart sensors (CT, Temperature, Occupancy) will pass the sensor data to cloud 
* Reduce power and maintenance cost with cloud analytics and machine learning algorithms to build SmartEnergySaver Controller for each home individually
* The system builds and executes the Intelligent Auto smart actions by combining the existing sensor data with other sensors like occupancy and Temperature sensors to control the appliance
* Accounting for more than 25% of monthly energy savings

## What are the impact metrics that one can use to analyze the effect of the solution?

* Device Usage trends by day,week,month
* What patterns emerge in your home and opportunity to saves
* Target for savings for entire home or by device by day,week,month in watts or dollars
* What's ON  : when the garage door opened, the television turned on, or when the dryer finished its cycle
* Your daily usage trends are more today. Please check out the devices. Turn on the Auto Smart action to save 10% daily.

## Frameworks/Tools/Technologies stacks to be used

* CT - Current sensor (YHDC SCT-013-030) - This sensor clamps over the main cable(MCB) in the house and transforms the magnetic field around the cable into a voltage
* Occupancy Sensor - The Occupancy sensors are devices that detect when a space is occupied/unoccupied. This sensor is placed on living, bedroom where high-power consuming devices are mounted
* Temperature Sensor – To detect current temperature in each rooms inside home
* ESP32 microcontroller, Arduino
* Azure Cloud for data analytics and machine learning

## Assumptions, constraints, and solution decision points (Reason behind choosing a technology)

* Serverless. using Azure cloud free account. This entire architecture falls under the Azure free tier. AWS will be cheaper . 
* Hardware Cost and Cloud pricing (pay as per you go model).
* Azure databricks used for distributed computing to do data analytics and machine learning(limitation on Azure free tier account).
* IOT hub sensor data push limitation
* Each home is different, however, so the number of detected devices may be higher or lower
* The identical device detection will take 3-6 months based on usage pattern.
* Need at least 1-3 months sensors data to build energy usage pattern 
* Need at least 1 year data to predict device maintanence and service

## How easily can your solution be implemented and how effective will it be?

* No wire-cutting and not putting a meter between every socket and appliances.
* Used Azure free Tier account for this prototype
* The hardware cost is minimal and for data analytics the cloud is required (distributed computing).The cloud cost can be reduced by using open source technologies
* Take measurements every 10 second to get an accurate  picture of electricity consumption.
* Save all the data in the cloud for later use & analytics.
* The application first trains for the existing appliances and then constantly shows the values of the electricity consumed individually for each appliances
* It gives statistics of electricity consumed by the individual appliance and cost associated


## Extent of Scalability/Usability

* Archive sensor data for machine learning and  future analytics 
* Able to handle multiple sensors (any sensors can be added in future)
* Save all the data in the cloud for later use & do use machine learning to build individual EnergyHomeController Model for predictive analysis
* Must be able to handle any data rate pushed to IOT Hub
* The machine learning algorithm begins to recognize the appliances and other devices that use more power like AC,fridge,Owen,TV.
* Security system is Armed away. You television turned on, Lights on. Do this need to be turned off?
* I think Freezer stopped working? Find out before your food goes to waste. Need maintenance. Call for service
* Your AC has been running for over an hour. Nobody there in Living room. Do this need to be turned off?
* Did you forgot to turn off the oven? 
* Additional selling feature for Security and Home automation product companies

# REALTIME USECASES

 ## AC Energy Comparison

 * The AC power usage was derived based on real problem faced by me due to more electricity bill
 * Air conditioners are one of the biggest energy hogs in the home, accounting for more than 25% of yearly electric use.
 * To consider a case of AC, it will study the usage patterns of AC, at what temperature it has been used and for what time. 
 * It will learn this data for a week, and then it will automatically power on and off the AC without human interference. 
 * The same can be applied to other appliances at home like geysers and fridge if required.
 * Turn on and off AC based on someone’s inside home based on usage pattern and analytics. 

 ## Intelligent Smart Action and Smart Alerts
 
 * These sensors learns about your home as things turn on and off.
 * The machine learning algorithm begins to recognize the appliances and other devices that use more power like AC,fridge,Owen,TV
 * Do you want turn on Smart Energy Mode to save 10% power daily
 * Intelligent Alerts : Forgot to turn off the AC? Forgot to turn off the Oven? 
 * Intelligent Alerts : Your AC has been running for over an hour. Nobody there in Living room. Do this need to be turned off?

## Proposed Workflow

  ![SelfieSmartSensorZ](https://user-images.githubusercontent.com/89841006/144635412-17cdd814-2846-489c-a1f4-5cc1a4c60298.png)

## Prototype - What we did 

* Hardware set up using CT, Occupancy(PIR sensor used for prototype) and temperature/humidity sensors
* Wiring the circuit for data collection
* Real time power usage data for the devices(AC,washing machine,fan,light) are collected for 15 days
* IOT simulator for 30 days(required for data analytics)
* Used Azure Cloud – Free Tier
* Created Azure IOT Hub to push sensor data
* Created Azure data bricks for streaming data analytics
* Azure blob and cosmos and SQL for Hot and cold data storage 
* Able to discover devices AC,fan,washing machine,light.For this prototype we have added this metadata
* Explored and identified machine learning algorithm for building up EneryHomecontroller Model for predictive analysis
* Web dashboard to show up energy usage and Intelligent smart alerts and smart actions

# Hardware/Software Cost


| Item              | Price(INR)        
| ----------------- | ------------------- 
| ESP32 arudino board            | 900   
| LCD Display (I2C) | 300  
| Protoboards       | 200  
| Headphone jacks   | 100
| Capacitor (10µF)  | 50 
| CT Sensor (YHDC SCT-013)   | 700  
| Resistors (between 10k and 470k Ω)/wires | 300    
| Total	cost        | INR 2350 ~ $31 
| Azure Cloud – Free Tier


## Wiring Diagram


  <img width="333" alt="CT-Sensor-Circuit-Diagram" src="https://user-images.githubusercontent.com/89841006/144637613-84e7b29f-dd3f-4c47-adeb-7f77aeb473f5.PNG">


## Video explanation

<div align="center">

*[https://www.youtube.com/watch?v=ah3ezprtgmc](https://www.youtube.com/watch?v=ah3ezprtgmc)*
</div>


## Cloud Architecture Diagram

This is the cloud architecture for the prototype:

   ![Cloud_Architecture](https://user-images.githubusercontent.com/89841006/144638424-81e47880-e8cb-4537-b3bd-1ff6b704fd58.png)


In a nutshell:
* The ESP32 has a MQTT connection with Azure IoT Core
* Every 10 seconds the power/occupancy/temperature measurements are sent to Azure. This happens 8640 times a day (6 times per minute).
* These measurements are stored in Cosmos 
* Once a day, all readings from the previous day are archived to SQL
* A GraphQL API (hosted on Lambda) exposes the data stored in Cosmos
* Machine learning will gradually learn about your home as things turn on and off
* Machine learning uses that data to determine what devices are on and off
* Have a simple app to visualize the data and analyze trends over time
* Generate Intelligent Smart Actions/Alerts based on data analytics and machine learning

## Screenshots

Web dashboard, built on top of the GraphQL API: 

Real time prototype Web dashboard URL : http://homeenergysavings.azurewebsites.net/

 <img width="954" alt="web_dashboard" src="https://user-images.githubusercontent.com/89841006/144638317-80401cb7-e5ce-4e0a-9f35-64486e87dbc6.PNG"> 

Hardware : CT Sensor -ESP32 OLED display:

![Screenshot ESP32 OLED](https://savjee.github.io/home-energy-monitor/readme-images/esp32-oled.jpg)


## Data Analytics – Machine Learning Algorithm

* 




## Demo Link
 
 * Web dashboard URL : http://homeenergysavings.azurewebsites.net/

# Future Improvements

## Better Energy Management

 * The improvements helps us save nearly 40% of the electricity monthly with Intelligent smart scenes with predictive analysis using different sensors like motion,airquality,security system
 * Additional selling feature for Security and Home automation product companies
 * Integration of Amazon Alexa/Google Home 
 
## Motor stalls – Pro Active service alert
 
 * In the home power critical devices like an AC or washing machine use motors.
 * When a stall occurs, the motor has stopped rotating even when there is enough voltage at its terminals.
 * This can indicate trouble for the future, serving as a warning of device failure ahead

## Power quality –Application Maintenance

 * Tracks voltage in real-time, average, and upper and lower bounds to give the user an idea of consistency of quality.
 * Recent spikes and dips. 
 * This alarms for service of the home appliances .
 * Proactive alert on the application Health check up and services



