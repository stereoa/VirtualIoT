# Virtual IoT Devices PoC

## Overview

This project is a **Proof of Concept (PoC)** for simulating IoT devices in C#. The goal is to model **real-world IoT architecture**—devices, sensors, and actuators—without needing physical hardware. It is designed to demonstrate **C#/.NET capabilities**, **MQTT communication**, and **device abstraction patterns** common in professional IoT systems.

The current implementation simulates a **smart parking scenario**, where each virtual parking spot behaves as a device with inputs (sensors) and outputs (actuators).

---

## Architecture

### 1. Virtual Device Layer

- **`IVirtualDevice`**: Interface representing any IoT device.
- **`VirtualDevice`**: Base class implementing MQTT connectivity, telemetry publishing, and actuator handling.
- **Derived Devices**: e.g., `VirtualParkingSpotDevice`, extend `VirtualDevice` to add spot-specific sensors (occupancy, temperature) and actuators (LED for indicator lights).

### 2. Sensors & Actuators

- **Sensors (`IVirtualSensor`)**
  - Provide device input data
  - Examples: `VirtualOccupiedSensor`, `VirtualTemperatureSensor`
- **Actuators (`IVirtualActuator`)**
  - Device outputs controlled via MQTT
  - Example: `VirtualLed`
  - Actuators are included in telemetry so dashboards can track outputs in real time.

### 3. MQTT Communication

- **MQTT broker**: Currently uses `test.mosquitto.org` for simulation
- **Topics**:
  - `parking/{deviceId}/telemetry` → device publishes sensor and actuator state
  - `parking/{deviceId}/command` → broker sends commands to actuators
- **Protocol features demonstrated**:
  - Publish/subscribe pattern
  - Topic-based messaging
  - Command handling for actuators

### 4. Device Management

- **`VirtualDeviceManager`**
  - Manages multiple `IVirtualDevice` instances
  - Supports scalable initialization and concurrent device loops
  - Provides polymorphic handling for heterogeneous devices (spots, gates, lights, etc.)

---

## Getting Started

1. Clone the repository
2. Install the `MQTTnet` NuGet package
3. Run `Program.cs`  
   - Initializes multiple parking spot devices
   - Connects to MQTT broker
   - Starts telemetry loops and command subscriptions

```csharp
var mqtt = new MqttService("test.mosquitto.org", 1883);
var manager = new VirtualDeviceManager();

// Add parking spots
for (int i = 1; i <= 5; i++)
{
    manager.AddDevice(new VirtualParkingSpotDevice($"spot{i}", mqtt));
}

await manager.InitializeAllAsync();
manager.RunAll();
```

## Project Roadmap

The current PoC is a **learning-focused simulation**, but it is designed to be extended toward real-world IoT scenarios. Future enhancements include:

1. **Real-time Dashboard**
   - Aggregate telemetry from multiple devices
   - Visualize parking occupancy and actuator states
   - Support live updates via console or web UI

2. **Advanced Device Types**
   - Parking gates with actuator control
   - Environmental sensors (light, CO2, temperature)
   - Integration with mobile or cloud apps

3. **Secure MQTT**
   - Support TLS/SSL encrypted communication
   - Add authentication for brokers

4. **Scalable Simulation**
   - Run dozens or hundreds of virtual devices concurrently
   - Simulate large parking facilities

5. **Data Logging & Analytics**
   - Save telemetry for historical analysis
   - Explore predictive occupancy patterns or anomaly detection

---

## Learning Objectives

This project helps expand **IoT knowledge in C#** by demonstrating:

- Device abstraction and polymorphism
- Sensor/actuator modeling in software
- MQTT communication patterns
- Concurrent device simulation
- Mapping software patterns to embedded/IoT architectures

---

## Key Takeaways

- Virtual devices behave **like real IoT devices**, enabling rapid prototyping without hardware
- Architecture supports **inputs and outputs**, multiple devices, and scalable deployment
