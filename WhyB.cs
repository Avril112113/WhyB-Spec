/* This file was generated from GenerateSpec/GeneratorCS.cs */

using Newtonsoft.Json;
using System;

/// <summary>
/// WhyB Version: 0.0.2
/// </summary>
namespace WhyB {
	public interface IWhyBBackend {
		/// <summary>
		/// Gets weather the given extension is supported
		/// </summary>
		/// <param name="name">SnakeCase name of the extension</param>
		/// <returns>Weather it's supported or not</returns>
		bool ExtensionSupported(string name);

		/// <summary>
		/// Sets the global simulation speed  
		/// Default simulation speed MUST be 0.2  
		/// The backend MUST support pausing (sim speed of 0) and sim speed of 0.2 (5 lines a second).
		/// </summary>
		/// <param name="speed">How frequent to update</param>
		/// <returns>Weather it succeeded or not</returns>
		bool SetSimulationSpeed(float speed);

		/// <summary>
		/// When requested, this will create a new empty network and respond with the new networks GUID
		/// </summary>
		/// <returns>The new network GUID</returns>
		Guid CreateNetwork();

		/// <summary>
		/// When requested, this will create a new device with default fields and respond with the new devices GUID  
		/// Device type naming see [Mandates](#Mandates)
		/// </summary>
		/// <param name="deviceType">The type of device to create</param>
		/// <param name="network">The GUID of the target network the new device will be in</param>
		/// <returns>The new device GUID</returns>
		Guid CreateDevice(string deviceType, Guid network);

		/// <summary>
		/// When requested, this will set the given field value in the network.  
		/// If `source` is not omitted then `source` is the device guid that the change originated, it MAY be used.
		/// </summary>
		/// <param name="network">The GUID of the target network</param>
		/// <param name="fieldName">The field name to change the value of</param>
		/// <param name="fieldValue">The new value for the field</param>
		/// <param name="source">The source device GUID of the change (can be omitted)</param>
		/// <returns>Weather it succeeded or not</returns>
		bool SetNetworkValue(Guid network, string fieldName, String fieldValue, string? source);

		/// <summary>
		/// When requested, this should set the field name on the device.  
		/// TODO: mandate what to do when the new name exists or does not exist in the network
		/// </summary>
		/// <param name="device">GUID of the device</param>
		/// <param name="fieldId">The field ID to change the name of</param>
		/// <param name="fieldName">The name to change to</param>
		/// <returns>Weather it succeeded or not</returns>
		bool SetDeviceFieldName(Guid device, string fieldId, string fieldName);
	}

	public interface IWhyBFrontend {
		/// <summary>
		/// Event is fired when a new device is created
		/// </summary>
		/// <param name="device">The GUID of the device of the new device</param>
		/// <param name="network">The GUID of the network the new device is in</param>
		/// <param name="deviceType">The type of device</param>
		void DeviceCreated(Guid device, Guid network, string deviceType);

		/// <summary>
		/// Event is fired when a devices field name has been changed
		/// </summary>
		/// <param name="device">The GUID of the device where the change originated</param>
		/// <param name="fieldId">The changed field ID</param>
		/// <param name="fieldName">The new field name</param>
		void DeviceFieldNameChanged(Guid device, string fieldId, string fieldName);

		/// <summary>
		/// Event is fired when a new network is created
		/// </summary>
		/// <param name="network">The GUID of the network that was created</param>
		void NetworkCreated(Guid network);

		/// <summary>
		/// Event is fired when a network's field value has changed
		/// </summary>
		/// <param name="network">The GUID of the network where the change originated</param>
		/// <param name="fieldName">The name of the field that changed</param>
		/// <param name="fieldValue">The new value of the field</param>
		void NetworkValueChanged(Guid network, string fieldName, String fieldValue);

		/// <summary>
		/// Event is fired the backends simulation speed has been changed
		/// </summary>
		/// <param name="simulationSpeed">The new simulation speed</param>
		void SimulationSpeedChanged(float simulationSpeed);
	}

	public interface IWhyBBackendSingleArgs {
		/// <summary>
		/// Gets weather the given extension is supported
		/// </summary>
		/// <param name="request">The request parameters</param>
		/// <returns>Weather it's supported or not</returns>
		bool ExtensionSupported(RequestExtensionSupported request);

		/// <summary>
		/// Sets the global simulation speed  
		/// Default simulation speed MUST be 0.2  
		/// The backend MUST support pausing (sim speed of 0) and sim speed of 0.2 (5 lines a second).
		/// </summary>
		/// <param name="request">The request parameters</param>
		/// <returns>Weather it succeeded or not</returns>
		bool SetSimulationSpeed(RequestSetSimulationSpeed request);

		/// <summary>
		/// When requested, this will create a new empty network and respond with the new networks GUID
		/// </summary>
		/// <param name="request">The request parameters</param>
		/// <returns>The new network GUID</returns>
		Guid CreateNetwork(RequestCreateNetwork request);

		/// <summary>
		/// When requested, this will create a new device with default fields and respond with the new devices GUID  
		/// Device type naming see [Mandates](#Mandates)
		/// </summary>
		/// <param name="request">The request parameters</param>
		/// <returns>The new device GUID</returns>
		Guid CreateDevice(RequestCreateDevice request);

		/// <summary>
		/// When requested, this will set the given field value in the network.  
		/// If `source` is not omitted then `source` is the device guid that the change originated, it MAY be used.
		/// </summary>
		/// <param name="request">The request parameters</param>
		/// <returns>Weather it succeeded or not</returns>
		bool SetNetworkValue(RequestSetNetworkValue request);

		/// <summary>
		/// When requested, this should set the field name on the device.  
		/// TODO: mandate what to do when the new name exists or does not exist in the network
		/// </summary>
		/// <param name="request">The request parameters</param>
		/// <returns>Weather it succeeded or not</returns>
		bool SetDeviceFieldName(RequestSetDeviceFieldName request);
	}

	public interface IWhyBFrontendSingleArgs {
		/// <summary>
		/// Event is fired when a new device is created
		/// </summary>
		/// <param name="data">The event parameters</param>
		void DeviceCreated(EventDeviceCreated data);

		/// <summary>
		/// Event is fired when a devices field name has been changed
		/// </summary>
		/// <param name="data">The event parameters</param>
		void DeviceFieldNameChanged(EventDeviceFieldNameChanged data);

		/// <summary>
		/// Event is fired when a new network is created
		/// </summary>
		/// <param name="data">The event parameters</param>
		void NetworkCreated(EventNetworkCreated data);

		/// <summary>
		/// Event is fired when a network's field value has changed
		/// </summary>
		/// <param name="data">The event parameters</param>
		void NetworkValueChanged(EventNetworkValueChanged data);

		/// <summary>
		/// Event is fired the backends simulation speed has been changed
		/// </summary>
		/// <param name="data">The event parameters</param>
		void SimulationSpeedChanged(EventSimulationSpeedChanged data);
	}

#region Extensions
	public interface IWhyBBackendChipDebugging {
		public const string Name = "chip_debugging";

		/// <summary>
		/// Gets or sets weather the chip is in debugging mode or not.  
		/// If `enabled` not omitted, set debugging mode to value of `enabled`
		/// </summary>
		/// <param name="chip">The GUID of the chip that is being ticked</param>
		/// <param name="enabled">Weather the debuggin mode should be enabled or disabled</param>
		/// <returns>Returns if this chip is in debugging mode (if changed, return new mode)</returns>
		bool Debug(string chip, bool? enabled);

		/// <summary>
		/// Run `count` lines of the chip
		/// </summary>
		/// <param name="chip">The GUID of the chip that is being ticked</param>
		/// <param name="count">Run this many lines</param>
		/// <returns>Always true</returns>
		bool TickLine(string chip, int count);

		/// <summary>
		/// Run `count` statements of the chip  
		/// MAY implement if able to be supported
		/// </summary>
		/// <param name="chip">The GUID of the chip that is being ticked</param>
		/// <param name="count">Run this many statements</param>
		/// <returns>The position of the cursor after running statements  
		/// `-1` to indicate that it reached the end and has moved to the next line</returns>
		int TickStmt(string chip, int count);

		/// <summary>
		/// Run until condition is true  
		/// MAY implement if able to be supported
		/// </summary>
		/// <param name="chip">The GUID of the chip that is being ticked</param>
		/// <param name="until">Run until `done != 0`  
		/// MUST contain same language as execution (normally YOLOL)</param>
		/// <returns>The line where the condition was met  
		/// </returns>
		int TickUntil(string chip, string until);
	}

	public interface IWhyBFrontendChipDebugging {
	}
	public interface IWhyBBackendChipDebuggingSingleArgs {
		public const string Name = "chip_debugging";

		/// <summary>
		/// Gets or sets weather the chip is in debugging mode or not.  
		/// If `enabled` not omitted, set debugging mode to value of `enabled`
		/// </summary>
		/// <param name="request">The request parameters</param>
		/// <returns>Returns if this chip is in debugging mode (if changed, return new mode)</returns>
		bool Debug(RequestDebug request);

		/// <summary>
		/// Run `count` lines of the chip
		/// </summary>
		/// <param name="request">The request parameters</param>
		/// <returns>Always true</returns>
		bool TickLine(RequestTickLine request);

		/// <summary>
		/// Run `count` statements of the chip  
		/// MAY implement if able to be supported
		/// </summary>
		/// <param name="request">The request parameters</param>
		/// <returns>The position of the cursor after running statements  
		/// `-1` to indicate that it reached the end and has moved to the next line</returns>
		int TickStmt(RequestTickStmt request);

		/// <summary>
		/// Run until condition is true  
		/// MAY implement if able to be supported
		/// </summary>
		/// <param name="request">The request parameters</param>
		/// <returns>The line where the condition was met  
		/// </returns>
		int TickUntil(RequestTickUntil request);
	}

	public interface IWhyBFrontendChipDebuggingSingleArgs {
	}
#endregion Extensions

#region RequestClasses
	/// <summary>
	/// Gets weather the given extension is supported
	/// </summary>
	public class RequestExtensionSupported {
		public RequestExtensionSupported(string name)
		{
			Name = name;
		}

		/// <summary>
		/// SnakeCase name of the extension
		/// </summary>
		[JsonProperty("name", Required = Required.Always)]
		public string Name { get; set; }
	}

	/// <summary>
	/// Sets the global simulation speed  
	/// Default simulation speed MUST be 0.2  
	/// The backend MUST support pausing (sim speed of 0) and sim speed of 0.2 (5 lines a second).
	/// </summary>
	public class RequestSetSimulationSpeed {
		public RequestSetSimulationSpeed(float speed)
		{
			Speed = speed;
		}

		/// <summary>
		/// How frequent to update
		/// </summary>
		[JsonProperty("speed", Required = Required.Always)]
		public float Speed { get; set; }
	}

	/// <summary>
	/// When requested, this will create a new empty network and respond with the new networks GUID
	/// </summary>
	public class RequestCreateNetwork {
		public RequestCreateNetwork()
		{
		}
	}

	/// <summary>
	/// When requested, this will create a new device with default fields and respond with the new devices GUID  
	/// Device type naming see [Mandates](#Mandates)
	/// </summary>
	public class RequestCreateDevice {
		public RequestCreateDevice(string deviceType, Guid network)
		{
			DeviceType = deviceType;
			Network = network;
		}

		/// <summary>
		/// The type of device to create
		/// </summary>
		[JsonProperty("device_type", Required = Required.Always)]
		public string DeviceType { get; set; }

		/// <summary>
		/// The GUID of the target network the new device will be in
		/// </summary>
		[JsonProperty("network", Required = Required.Always)]
		public Guid Network { get; set; }
	}

	/// <summary>
	/// When requested, this will set the given field value in the network.  
	/// If `source` is not omitted then `source` is the device guid that the change originated, it MAY be used.
	/// </summary>
	public class RequestSetNetworkValue {
		public RequestSetNetworkValue(Guid network, string fieldName, String fieldValue, string? source)
		{
			Network = network;
			FieldName = fieldName;
			FieldValue = fieldValue;
			Source = source;
		}

		/// <summary>
		/// The GUID of the target network
		/// </summary>
		[JsonProperty("network", Required = Required.Always)]
		public Guid Network { get; set; }

		/// <summary>
		/// The field name to change the value of
		/// </summary>
		[JsonProperty("field_name", Required = Required.Always)]
		public string FieldName { get; set; }

		/// <summary>
		/// The new value for the field
		/// </summary>
		[JsonProperty("field_value", Required = Required.Always)]
		public String FieldValue { get; set; }

		/// <summary>
		/// The source device GUID of the change (can be omitted)
		/// </summary>
		[JsonProperty("source")]
		public string? Source { get; set; }
	}

	/// <summary>
	/// When requested, this should set the field name on the device.  
	/// TODO: mandate what to do when the new name exists or does not exist in the network
	/// </summary>
	public class RequestSetDeviceFieldName {
		public RequestSetDeviceFieldName(Guid device, string fieldId, string fieldName)
		{
			Device = device;
			FieldId = fieldId;
			FieldName = fieldName;
		}

		/// <summary>
		/// GUID of the device
		/// </summary>
		[JsonProperty("device", Required = Required.Always)]
		public Guid Device { get; set; }

		/// <summary>
		/// The field ID to change the name of
		/// </summary>
		[JsonProperty("field_id", Required = Required.Always)]
		public string FieldId { get; set; }

		/// <summary>
		/// The name to change to
		/// </summary>
		[JsonProperty("field_name", Required = Required.Always)]
		public string FieldName { get; set; }
	}

	/// <summary>
	/// Gets or sets weather the chip is in debugging mode or not.  
	/// If `enabled` not omitted, set debugging mode to value of `enabled`
	/// </summary>
	public class RequestDebug {
		public RequestDebug(string chip, bool? enabled)
		{
			Chip = chip;
			Enabled = enabled;
		}

		/// <summary>
		/// The GUID of the chip that is being ticked
		/// </summary>
		[JsonProperty("chip", Required = Required.Always)]
		public string Chip { get; set; }

		/// <summary>
		/// Weather the debuggin mode should be enabled or disabled
		/// </summary>
		[JsonProperty("enabled")]
		public bool? Enabled { get; set; }
	}

	/// <summary>
	/// Run `count` lines of the chip
	/// </summary>
	public class RequestTickLine {
		public RequestTickLine(string chip, int count)
		{
			Chip = chip;
			Count = count;
		}

		/// <summary>
		/// The GUID of the chip that is being ticked
		/// </summary>
		[JsonProperty("chip", Required = Required.Always)]
		public string Chip { get; set; }

		/// <summary>
		/// Run this many lines
		/// </summary>
		[JsonProperty("count", Required = Required.Always)]
		public int Count { get; set; }
	}

	/// <summary>
	/// Run `count` statements of the chip  
	/// MAY implement if able to be supported
	/// </summary>
	public class RequestTickStmt {
		public RequestTickStmt(string chip, int count)
		{
			Chip = chip;
			Count = count;
		}

		/// <summary>
		/// The GUID of the chip that is being ticked
		/// </summary>
		[JsonProperty("chip", Required = Required.Always)]
		public string Chip { get; set; }

		/// <summary>
		/// Run this many statements
		/// </summary>
		[JsonProperty("count", Required = Required.Always)]
		public int Count { get; set; }
	}

	/// <summary>
	/// Run until condition is true  
	/// MAY implement if able to be supported
	/// </summary>
	public class RequestTickUntil {
		public RequestTickUntil(string chip, string until)
		{
			Chip = chip;
			Until = until;
		}

		/// <summary>
		/// The GUID of the chip that is being ticked
		/// </summary>
		[JsonProperty("chip", Required = Required.Always)]
		public string Chip { get; set; }

		/// <summary>
		/// Run until `done != 0`  
		/// MUST contain same language as execution (normally YOLOL)
		/// </summary>
		[JsonProperty("until", Required = Required.Always)]
		public string Until { get; set; }
	}
#endregion RequestClasses

#region EventClasses
	/// <summary>
	/// Event is fired when a new device is created
	/// </summary>
	public class EventDeviceCreated {
		public EventDeviceCreated(Guid device, Guid network, string deviceType)
		{
			Device = device;
			Network = network;
			DeviceType = deviceType;
		}

		/// <summary>
		/// The GUID of the device of the new device
		/// </summary>
		[JsonProperty("device", Required = Required.Always)]
		public Guid Device { get; set; }

		/// <summary>
		/// The GUID of the network the new device is in
		/// </summary>
		[JsonProperty("network", Required = Required.Always)]
		public Guid Network { get; set; }

		/// <summary>
		/// The type of device
		/// </summary>
		[JsonProperty("device_type", Required = Required.Always)]
		public string DeviceType { get; set; }
	}

	/// <summary>
	/// Event is fired when a devices field name has been changed
	/// </summary>
	public class EventDeviceFieldNameChanged {
		public EventDeviceFieldNameChanged(Guid device, string fieldId, string fieldName)
		{
			Device = device;
			FieldId = fieldId;
			FieldName = fieldName;
		}

		/// <summary>
		/// The GUID of the device where the change originated
		/// </summary>
		[JsonProperty("device", Required = Required.Always)]
		public Guid Device { get; set; }

		/// <summary>
		/// The changed field ID
		/// </summary>
		[JsonProperty("field_id", Required = Required.Always)]
		public string FieldId { get; set; }

		/// <summary>
		/// The new field name
		/// </summary>
		[JsonProperty("field_name", Required = Required.Always)]
		public string FieldName { get; set; }
	}

	/// <summary>
	/// Event is fired when a new network is created
	/// </summary>
	public class EventNetworkCreated {
		public EventNetworkCreated(Guid network)
		{
			Network = network;
		}

		/// <summary>
		/// The GUID of the network that was created
		/// </summary>
		[JsonProperty("network", Required = Required.Always)]
		public Guid Network { get; set; }
	}

	/// <summary>
	/// Event is fired when a network's field value has changed
	/// </summary>
	public class EventNetworkValueChanged {
		public EventNetworkValueChanged(Guid network, string fieldName, String fieldValue)
		{
			Network = network;
			FieldName = fieldName;
			FieldValue = fieldValue;
		}

		/// <summary>
		/// The GUID of the network where the change originated
		/// </summary>
		[JsonProperty("network", Required = Required.Always)]
		public Guid Network { get; set; }

		/// <summary>
		/// The name of the field that changed
		/// </summary>
		[JsonProperty("field_name", Required = Required.Always)]
		public string FieldName { get; set; }

		/// <summary>
		/// The new value of the field
		/// </summary>
		[JsonProperty("field_value", Required = Required.Always)]
		public String FieldValue { get; set; }
	}

	/// <summary>
	/// Event is fired the backends simulation speed has been changed
	/// </summary>
	public class EventSimulationSpeedChanged {
		public EventSimulationSpeedChanged(float simulationSpeed)
		{
			SimulationSpeed = simulationSpeed;
		}

		/// <summary>
		/// The new simulation speed
		/// </summary>
		[JsonProperty("simulation_speed", Required = Required.Always)]
		public float SimulationSpeed { get; set; }
	}
#endregion EventClasses
}
