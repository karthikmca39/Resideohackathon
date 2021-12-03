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
* Save all the data in the cloud for later use & do use machine learning to build EnergyHomeController for predictive analysis
* Must be able to handle any data rate pushed to IOT Hub
* The machine learning algorithm begins to recognize the appliances and other devices that use more power like AC,fridge,Owen,TV.
* Security system is Armed away. You television turned on, Lights on. Do this need to be turned off?
* I think Freezer stopped working? Find out before your food goes to waste. Need maintenance. Call for service
* Your AC has been running for over an hour. Nobody there in Living room. Do this need to be turned off?
* Did you forgot to turn off the oven? 
* Additional selling feature for Security and Home automation product companies

# USECASE

 ## AC Energy Comparison

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

## Prototype -What we did 

* Hardware set up using CT, Occupancy and temperature sensors
* Wiring the circuit for data collection
* IOT simulator for 15 days(required for data analytics)
* Used Azure Cloud – Free Tier
* Created Azure IOT Hub to push sensor data
* Created Azure data bricks for streaming data analytics
* Azure blob and cosmos and SQL for Hot and cold data storage 
* Explored and identified machine learning algorithm for building up EneryHomecontroller Model for predictive analysis
* Web dashboard to show up energy usage and Intelligent smart alerts and smart actions

# Hardware/Software Cost

| Item              | Price        
| ----------------- | ------------------- 
| ESP32        | Mobile app (Ionic)   
| `src-aws`         | Serverless AWS backend + GraphQL API  
| `src-esp32`       | Firmware for the ESP32 (measuring device)  

## Structure


This project consists out of multiple components:

| Folder            | Description         | Build status | 
| ----------------- | ------------------- | ------------ | 
| `src-app`         | Mobile app (Ionic)  | n/a |
| `src-aws`         | Serverless AWS backend + GraphQL API | ![AWS Build Status](https://github.com/Savjee/home-energy-monitor/workflows/aws/badge.svg) |
| `src-esp32`       | Firmware for the ESP32 (measuring device) | ![Firmware Build Status](https://github.com/Savjee/home-energy-monitor/workflows/firmware/badge.svg) |

(TODO: add instructions on how to deploy all of this. 😅)

## Video explanation

<div align="center">

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/ah3ezprtgmc/0.jpg)](https://www.youtube.com/watch?v=ah3ezprtgmc)

*[https://www.youtube.com/watch?v=ah3ezprtgmc](https://www.youtube.com/watch?v=ah3ezprtgmc)*
</div>

Read my blog post for more instructions: [https://savjee.be/2019/07/Home-Energy-Monitor-ESP32-CT-Sensor-Emonlib/](https://savjee.be/2019/07/Home-Energy-Monitor-ESP32-CT-Sensor-Emonlib/)

## Cloud Architecture

This is the cloud architecture that powers the energy meter and the app:

![AWS Cloud Architecture](https://savjee.github.io/home-energy-monitor/readme-images/architecture.png)

In a nutshell:
* The ESP32 has a MQTT connection with AWS IoT Core
* Every 30 seconds, 30 measurements are sent to AWS
* These measurements are stored in DynamoDB (IoT Rule)
* Once a day, all readings from the previous day are archived to S3
* A GraphQL API (hosted on Lambda) exposes the data stored in DynamoDB

## Screenshots

Web dashboard, built on top of the GraphQL API:

![Screenshot Web Dashboard](https://savjee.github.io/home-energy-monitor/readme-images/web-dashboard.png)

What is displayed on the ESP32 OLED display:

![Screenshot ESP32 OLED](https://savjee.github.io/home-energy-monitor/readme-images/esp32-oled.jpg)


## DIY Requirements

To build your own Energy Monitor you need the following hardware:

* ESP32
* CT sensor: YHDC SCT-013-030 (30A/1V)
* 10µF capacitor
* 2 resistors (between 10k-470kΩ)

Other requirements:
* AWS Account (Should be able to run in free-tier)
* Install [PlatformIO](https://platformio.org) on your system
* Drivers for your ESP32 board

Read my blog post for more instructions: [https://savjee.be/2019/07/Home-Energy-Monitor-ESP32-CT-Sensor-Emonlib/](https://savjee.be/2019/07/Home-Energy-Monitor-ESP32-CT-Sensor-Emonlib/)


## Contribute

I'm happy to merge in any pull requests. Also feel free to report bugs or feature requests.
