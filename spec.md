[comment]: # "This file was generated from GenerateSpec/GeneratorMD.cs"  
# **Specification**  
Version: 0.0.2  
This spec uses MUST, MUST NOT, SHOULD, SHOULD NOT, MAY and MAY NOT as described in https://www.ietf.org/rfc/rfc2119.txt  

- [Specification](#Specification)  
- [Mandates](#Mandates)  
- [Codes](#Codes)  
- [Requests](#Requests)  
	- [**`extension_supported`**](#extension_supported)  
	- [**`set_simulation_speed`**](#set_simulation_speed)  
	- [**`create_network`**](#create_network)  
	- [**`create_device`**](#create_device)  
	- [**`set_network_value`**](#set_network_value)  
	- [**`set_device_field_name`**](#set_device_field_name)  
- [Events](#Events)  
	- [**`device_created`**](#device_created)  
	- [**`device_field_name_changed`**](#device_field_name_changed)  
	- [**`network_created`**](#network_created)  
	- [**`network_value_changed`**](#network_value_changed)  
	- [**`simulation_speed_changed`**](#simulation_speed_changed)  
- [**Extensions**](#Extensions)  
	- [**chip_debugging**](#chip_debugging)  
		- [Requests](#Requests%20(chip_debugging))  
			- [**`debug`**](#debug)  
			- [**`tick_line`**](#tick_line)  
			- [**`tick_stmt`**](#tick_stmt)  
			- [**`tick_until`**](#tick_until)  


# **Mandates**  
Communication using JsonRPC over WebSockets.  
Device types MUST use snake case, examples are `button`, `lamp` and `range_finder`.  
Field ID's MUST use PascalCase, examples are `ButtonState` and `LampOn`.  
Field names MUST only contain alphanumerical characters and underscores, examples are `my_lamp_state`, `buttonstate` and `door1_state`.  
Upon a new client, the backend MUST send the new client the current state though events.  


# **Codes**  
|  Code   |  Meaning  
|:-------:|:-----------------------------------  
| -32000  | The backend had an unexpected error  


# **Requests**  
## **`extension_supported`**  
Gets weather the given extension is supported  
**Request:**  
```jsonc
{
	"jsonrpc": "2.0",
	"method": "extension_supported",
	"params": {
		// SnakeCase name of the extension
		"name": "string",
	},
	"id": 1
}
```  
**Response:**  
```jsonc
{
	"jsonrpc": "2.0",
	// Weather it's supported or not
	"result": "bool",
	"id": 1
}
```  


## **`set_simulation_speed`**  
Sets the global simulation speed  
Default simulation speed MUST be 0.2  
The backend MUST support pausing (sim speed of 0) and sim speed of 0.2 (5 lines a second).  
**Request:**  
```jsonc
{
	"jsonrpc": "2.0",
	"method": "set_simulation_speed",
	"params": {
		// How frequent to update
		"speed": "float",
	},
	"id": 1
}
```  
**Response:**  
```jsonc
{
	"jsonrpc": "2.0",
	// Weather it succeeded or not
	"result": "bool",
	"id": 1
}
```  


## **`create_network`**  
When requested, this will create a new empty network and respond with the new networks GUID  
**Request:**  
```jsonc
{
	"jsonrpc": "2.0",
	"method": "create_network",
	"id": 1
}
```  
**Response:**  
```jsonc
{
	"jsonrpc": "2.0",
	// The new network GUID
	"result": "Network",
	"id": 1
}
```  


## **`create_device`**  
When requested, this will create a new device with default fields and respond with the new devices GUID  
Device type naming see [Mandates](#Mandates)  
**Request:**  
```jsonc
{
	"jsonrpc": "2.0",
	"method": "create_device",
	"params": {
		// The type of device to create
		"device_type": "string",
		// The GUID of the target network the new device will be in
		"network": "Network",
	},
	"id": 1
}
```  
**Response:**  
```jsonc
{
	"jsonrpc": "2.0",
	// The new device GUID
	"result": "Device",
	"id": 1
}
```  


## **`set_network_value`**  
When requested, this will set the given field value in the network.  
If `source` is not omitted then `source` is the device guid that the change originated, it MAY be used.  
**Request:**  
```jsonc
{
	"jsonrpc": "2.0",
	"method": "set_network_value",
	"params": {
		// The GUID of the target network
		"network": "Network",
		// The field name to change the value of
		"field_name": "string",
		// The new value for the field
		"field_value": "YololValue",
		// The source device GUID of the change (can be omitted)
		"source": "string",
	},
	"id": 1
}
```  
**Response:**  
```jsonc
{
	"jsonrpc": "2.0",
	// Weather it succeeded or not
	"result": "bool",
	"id": 1
}
```  


## **`set_device_field_name`**  
When requested, this should set the field name on the device.  
TODO: mandate what to do when the new name exists or does not exist in the network  
**Request:**  
```jsonc
{
	"jsonrpc": "2.0",
	"method": "set_device_field_name",
	"params": {
		// GUID of the device
		"device": "Device",
		// The field ID to change the name of
		"field_id": "string",
		// The name to change to
		"field_name": "string",
	},
	"id": 1
}
```  
**Response:**  
```jsonc
{
	"jsonrpc": "2.0",
	// Weather it succeeded or not
	"result": "bool",
	"id": 1
}
```  


# **Events**  
## **`device_created`**  
Event is fired when a new device is created  
**Event Data:**  
```jsonc
{
	// The GUID of the device of the new device
	"device": "Device",
	// The GUID of the network the new device is in
	"network": "Network",
	// The type of device
	"device_type": "string",
}
```  


## **`device_field_name_changed`**  
Event is fired when a devices field name has been changed  
**Event Data:**  
```jsonc
{
	// The GUID of the device where the change originated
	"device": "Device",
	// The changed field ID
	"field_id": "string",
	// The new field name
	"field_name": "string",
}
```  


## **`network_created`**  
Event is fired when a new network is created  
**Event Data:**  
```jsonc
{
	// The GUID of the network that was created
	"network": "Network",
}
```  


## **`network_value_changed`**  
Event is fired when a network's field value has changed  
**Event Data:**  
```jsonc
{
	// The GUID of the network where the change originated
	"network": "Network",
	// The name of the field that changed
	"field_name": "string",
	// The new value of the field
	"field_value": "YololValue",
}
```  


## **`simulation_speed_changed`**  
Event is fired the backends simulation speed has been changed  
**Event Data:**  
```jsonc
{
	// The new simulation speed
	"simulation_speed": "float",
}
```  


# **Extensions**  
Extensions are standard optional features sets, it is recommended to implement all of these if possible.  

# chip_debugging  
Chip debugging mode, used for controlling execution of a chip and gaining extra info on it's state  
When a chip is in debugging mode it should ignore the global sim speed  

# Requests (chip_debugging)  
## **`debug`**  
Gets or sets weather the chip is in debugging mode or not.  
If `enabled` not omitted, set debugging mode to value of `enabled`  
**Request:**  
```jsonc
{
	"jsonrpc": "2.0",
	"method": "debug",
	"params": {
		// The GUID of the chip that is being ticked
		"chip": "string",
		// Weather the debuggin mode should be enabled or disabled
		"enabled": "bool",
	},
	"id": 1
}
```  
**Response:**  
```jsonc
{
	"jsonrpc": "2.0",
	// Returns if this chip is in debugging mode (if changed, return new mode)
	"result": "bool",
	"id": 1
}
```  


## **`tick_line`**  
Run `count` lines of the chip  
**Request:**  
```jsonc
{
	"jsonrpc": "2.0",
	"method": "tick_line",
	"params": {
		// The GUID of the chip that is being ticked
		"chip": "string",
		// Run this many lines
		"count": "int",
	},
	"id": 1
}
```  
**Response:**  
```jsonc
{
	"jsonrpc": "2.0",
	// Always true
	"result": "bool",
	"id": 1
}
```  


## **`tick_stmt`**  
Run `count` statements of the chip  
MAY implement if able to be supported  
**Request:**  
```jsonc
{
	"jsonrpc": "2.0",
	"method": "tick_stmt",
	"params": {
		// The GUID of the chip that is being ticked
		"chip": "string",
		// Run this many statements
		"count": "int",
	},
	"id": 1
}
```  
**Response:**  
```jsonc
{
	"jsonrpc": "2.0",
	// The position of the cursor after running statements  
	// `-1` to indicate that it reached the end and has moved to the next line
	"result": "int",
	"id": 1
}
```  


## **`tick_until`**  
Run until condition is true  
MAY implement if able to be supported  
**Request:**  
```jsonc
{
	"jsonrpc": "2.0",
	"method": "tick_until",
	"params": {
		// The GUID of the chip that is being ticked
		"chip": "string",
		// Run until `done != 0`  
		// MUST contain same language as execution (normally YOLOL)
		"until": "string",
	},
	"id": 1
}
```  
**Response:**  
```jsonc
{
	"jsonrpc": "2.0",
	// The line where the condition was met  
	// 
	"result": "int",
	"id": 1
}
```  



