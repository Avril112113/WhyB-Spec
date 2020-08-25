# Specification
Version: 0.0.1-pre
This spec uses `MUST`, `MUST NOT`, `SHOULD`, `SHOULD NOT`, `MAY` and `MAY NOT` as described in https://www.ietf.org/rfc/rfc2119.txt
A `YololValue` in JSON is either a number or a string, sending any other data is not supported by this spec  

Table Of Contents:  
- [Specification](#specification)
- [Mandates](#mandates)
- [Error Codes](#error-codes)
- [Chip execution ticking](#chip-execution-ticking)
- [Subscriptions](#subscriptions)
	- [Subscribe Request](#subscribe-request)
	- [List of available subscriptions](#list-of-available-subscriptions)
	- [**`device_created`**](#device_created)
	- [**`device_field_name_changed`**](#device_field_name_changed)
	- [**`network_created`**](#network_created)
	- [**`network_value_changed`**](#network_value_changed)
- [State modification requests](#state-modification-requests)
	- [**`create_network`**](#create_network)
	- [**`create_device`**](#create_device)
	- [**`set_network_value`**](#set_network_value)
	- [**`set_device_field_name`**](#set_device_field_name)


# Mandates
This spec mandates the use of JsonRPC over WebSockets, see [Protocol](#Protocol) for implementation details.  
Device types MUST use snake case, examples; `button`, `lamp` and `range_finder`.  
Field ID's MUST use PascalCase, examples; `ButtonState`, `LampOn`.  
Field names MUST only contain alphanumerical characters and underscores, examples; `my_lamp_state`, `buttonstate`, `door1_state`.  
The backend MUST start with a blank state, no networks or devices.  
The backend MUST support pausing (speed of `0`) and running at 5 lines per second.  


# Error Codes
These codes are based off HTTP status codes  
*JsonRPC codes MUST always be used before considering a HTTP code*  
| Code |  Message                |  Meaning
|:----:|:----------------------- |:---------
| 500  | Internal Backend Error  | The backend had an unexpected error
| 551  | Invalid Subscribe Event | The passed event to be subscribed is invalid
| 552  | Until expr unsupported  | The server does not support 'ticking until evaluates true' expression


# Chip execution ticking
**Sent from frontend to backend**  
Control the execution of YOLOL  

**Request:**
```json
// Set execution speed:
{
	"jsonrpc": "2.0",
	"method": "tick_chip",
	"params": {
		// The GUID of the chip that is being ticked
		"chip": "3f72b0a3-70d0-4815-b41d-123d191538db",

		// Run this many ticks (or lines of yolol)
		"count": 5,
		// - OR -
		// Run until this expression evaluates to true
		// backends MAY support this but is recommended
		"until": ":done != 0"
	},
	"id": 1
}
```

**Response:**
```json
{
	"jsonrpc": "2.0",
	"result": true,
	"id": 1
}
```


# Subscriptions
Subscriptions are used to get information updates from the backend without the need to ask for them  

## Subscribe Request
- `<SUBSCRIBABLE>` is one of the subscribables listed below
- `<SUBSCRIBABLE_PARAMETERS>` is the subscribe parameters specific to that subscribable
```json
// Frontend -> Backend
// Subscribe for updates:
{
	"jsonrpc": "2.0",
	"method": "subscribe",
	"params": {
		"type": "<SUBSCRIBABLE>",
		<SUBSCRIBABLE_PARAMETERS>
	}
}
```

## List of available subscriptions
## **`device_created`**
This event is fired when a new device is created  

**Subscribe Parameters:**
```json
{
	"type": "device_created"
}
```

**Event Parameters:**
```json
{
	// The GUID of the device
	"device": "a46cbab4-c27c-4d33-82b0-7c0a1c162356",
	// The GUID of the network the device is in
	"network": "9af5497a-bd54-4899-ba29-d97b545225a7",
	// The type of device
	"device_type": "lamp"
}
```


## **`device_field_name_changed`**
This event is fired when a devices field name has been changed  

**Subscribe Parameters:**
```json
{
	"type": "device_field_name_changed",
	// The GUID of the device
	"device": "a46cbab4-c27c-4d33-82b0-7c0a1c162356"
}
```

**Event Parameters:**
```json
{
	// The GUID of the device
	"device": "a46cbab4-c27c-4d33-82b0-7c0a1c162356",
	// The changed field ID
	"field_id": "LampOn",
	// The new field name
	"field_name": "my_lamp_state"
}
```


## **`network_created`**
This event is fired when a new network is created  

**Subscribe Parameters:**
```json
{
	"type": "network_created"
}
```

**Event Parameters:**
```json
{
	// The GUID of the network
	"network": "9af5497a-bd54-4899-ba29-d97b545225a7"
}
```


## **`network_value_changed`**
This event is fired when a network's field value has changed  

**Subscribe Parameters:**
```json
{
	"type": "network_created",
	// The GUID of the network
	"network": "9af5497a-bd54-4899-ba29-d97b545225a7"
}
```

**Event Parameters:**
```json
{
	// The GUID of the network
	"network": "9af5497a-bd54-4899-ba29-d97b545225a7",
	// The name of the field that changed
	"field_name": "my_lamp_state",
	// The new value of the field (YololValue)
	"field_value": 1
}
```


## Event Examples  <!-- omit in toc -->
```json
// Backend -> Frontend
// On device field name changed:
{
	"jsonrpc": "2.0",
	"method": "device_field_name_changed",
	"params": {
		"device": "a46cbab4-c27c-4d33-82b0-7c0a1c162356",
		"field_id": "LampOn",
		"field_name": "my_lamp_state"
	}
}

// Backend -> Frontend
// On network field value changed:
{
	"jsonrpc": "2.0",
	"method": "network_value_changed",
	"params": {
		"network": "9af5497a-bd54-4899-ba29-d97b545225a7",
		"field_name": "MyLittleButtonState",
		"field_value": 1
	}
}
```


# State modification requests
## **`create_network`**
**Sent from frontend to backend**  
When requested, this will create a new empty network and respond with the new networks GUID  

**Request:**
```json
{
	"jsonrpc": "2.0",
	"method": "create_network",
	"id": 1
}
```

**Response:**
```json
{
	"jsonrpc": "2.0",
	// The new network GUID
	"result": "9af5497a-bd54-4899-ba29-d97b545225a7",
	"id": 1
}
```

## **`create_device`**
**Sent from frontend to backend**
When requested, this will create a new device with default fields and respond with the new devices GUID  
Device type naming see [Mandates](#Mandates)  

**Request:**
```json
{
	"jsonrpc": "2.0",
	"method": "create_device",
	"params": {
		// The type of device to create
		"device_type": "button"
	},
	"id": 1
}
```

**Response:**
```json
{
	"jsonrpc": "2.0",
	// The new device GUID
	"result": "a46cbab4-c27c-4d33-82b0-7c0a1c162356",
	"id": 1
}
```


## **`set_network_value`**
**Sent from frontend to backend**
When requested, this will set the given field value in the network.  
If `source` is not omitted then the change must originate from that device, though the use of this field is optional.  

**Request:**
```json
{
	"jsonrpc": "2.0",
	"method": "set_network_value",
	"params": {
		// The GUID of the target network
		"network": "9af5497a-bd54-4899-ba29-d97b545225a7",
		// The field name to change the value of
		"field_name": "my_lamp_state",
		// The new value for the field
		"field_value": 1,
		// The source device GUID of the change (can be omitted)
		"source": "a46cbab4-c27c-4d33-82b0-7c0a1c162356"
	},
	"id": 1
}
```

**Response:**
```json
{
	"jsonrpc": "2.0",
	"result": true,
	"id": 1
}
```

## **`set_device_field_name`**
**Sent from frontend to backend**
When requested, this should set the field name on the device.  
TODO: mandate what to do when the new name exists or does not exist in the network

**Request:**
```json
{
	"jsonrpc": "2.0",
	"method": "set_device_field_name",
	"params": {
		// GUID of the device
		"device": "a46cbab4-c27c-4d33-82b0-7c0a1c162356",
		// The field ID to change the name of
		"field_id": "LampOn",
		// The name to change to
		"field_name": "my_lamp_state"
	},
	"id": 1
}
```

**Response:**
```json
{
	"jsonrpc": "2.0",
	"result": true,
	"id": 1
}
```
