# Specification
### Version: 0.1 pre
A `YOLOL value` in JSON is either a number or a string, send only other data is not supported by this spec

# Info on YololShipSystemSpec
WhyB spec uses [YololShipSystemSpec](https://github.com/martindevans/YololShipSystemSpec), though that spec uses yaml, it must be used as JSON for this spec.  
When serializing using that spec, a tag is used to specify a device type eg `!button` replace that with a field in the object named `type` eg `{"type": "button"}`.  
It is recommended that if you save the state to a file, you should use [YololShipSystemSpec](https://github.com/martindevans/YololShipSystemSpec) and save as a yaml file, though this is **not** mandated.  

# Mandates
This spec mandates the use of JsonRPC over TCP, see [JsonRPC Over TCP](#JsonRPC-Over-TCP).  
Version of [YololShipSystemSpec](https://github.com/martindevans/YololShipSystemSpec) to be used is: TBE  
Device types use snake case, for examples; `button`, `lamp` and `range_finder`, this style is the same as used in [YololShipSystemSpec](https://github.com/martindevans/YololShipSystemSpec)  

# JsonRPC Over TCP
Following how HTTP uses headers only one header is valid `Content-Length` and should always be used  
Incoming messages should always be UTF-8  
An example of a message (**Note**: use of tabs, not spaces)  
```json
Content-Length: 45\r\n
\r\n
{
	"jsonrpc": "2.0",
	"method": "METHOD"
}
```
The use of batched JsonRPC is unsupported  

# Error Codes
|  Code  |  Message                |  Meaning
|:------:|:----------------------- |:---------
| -22700 | Invalid State Format    | The state given to `load_state` was formatted incorrectly
| -22800 | Invalid Subscribe Event | The passed event to be subscribed is invalid
| -22603 | Internal Backend Error  | The backend had an unexpected error

# Sate serialization
When a request is sent to the backend to load state, it should still fire subscribed events

**Examples:**
- `<STATE>` is the json version of the save state based on [YololShipSystemSpec](https://github.com/martindevans/YololShipSystemSpec)  
```json
Frontend -> Backend
Request to load state:
{
	"jsonrpc": "2.0",
	"method": "load_state",
	"params": {
		"state": <STATE>
	},
	"id": 1
}

Success Response:
{
	"jsonrpc": "2.0",
	"result": true,
	"id": 1
}

Error Response: (Normal JsonRPC error)
{
	"jsonrpc": "2.0",
	"error": {"code": -22700, "message": "Invalid State Format"},
	"id": 1
}
```
```json
Frontend -> Backend
Request to save state
{
	"jsonrpc": "2.0",
	"method": "save_state",
	"id": 1
}

Response:
{
	"jsonrpc": "2.0",
	"result": <STATE>,
	"id": 1
}
```

# Subscriptions
Subscriptions are a way to get updates on changed information without having to request for them  

**Examples:**
- `<DEVICE_GUID>` is the devices GUID
- `<SUBSCRIBABLE>` is one of the [subscribables](#subscribables)
- `<NETWORK_GUID>` is the devices GUID  
- `<SUBSCRIBABLE PARAMETERS>` is the subscribe parameters specific to that [subscribable](#subscribables)
```json
Frontend -> Backend
Subscribe for updates:
{
	"jsonrpc": "2.0",
	"method": "subscribe",
	"params": {
		"type": "<SUBSCRIBABLE>",
		<SUBSCRIBABLE PARAMETERS>
	}
}
```

# Subscribables
Subscribables are used to get information updates from the backend without the need to ask for them, see [Subscriptions](#Subscriptions)  
The following are available subscribables  

## **`device_created`**
**Event Source:** Global  
This event is fired when a new device is created  

**Subscribe Parameters:**  
None  

**Event Parameters:**  
`device`: The GUID of the device  
`network`: The GUID of the network the device is in  
`type`: The type of device  


## **`device_field_name_changed`**
**Event Source:** Device  
This event is fired when a devices field name has been changed

**Subscribe Parameters:**  
`device`: The GUID of the device  

**Event Parameters:**  
`device`: The GUID of the device  
`field_id`: The identity of the field (the default name)  
`field_name`: The new name of the field  

## **`network_created`**
**Event Source:** Global  
This event is fired when a new network is created  

**Subscribe Parameters:**  
None  

**Event Parameters:**  
`network`: The GUID of the network  

## **`network_value_changed`**
**Event Source:** Network  
This event is fired when a networks field value has changed  

**Subscribe Parameters:**  
`network`: The GUID of the network  

**Event Parameters:**  
`network`: The GUID of the network  
`field_name`: The name of the field that changed  
`field_value`: The new value of the field (YOLOL value)  

## Full Examples:  
```json
Backend -> Frontend
On device field name changed:
{
	"jsonrpc": "2.0",
	"method": "device_field_name_changed",
	"params": {
		"device": "a46cbab4-c27c-4d33-82b0-7c0a1c162356",
		"field_id": "ButtonState",
		"field_name": "MyLittleButtonState"
	}
}

Backend -> Frontend
On network field value changed:
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

# Simulation speed control
`lines_second` can be `-1`, this indicates run as fast as possible; otherwise if it's positive, run this many lines per second  
**Full Example:**
```json
Frontend -> Backend
Set execution speed:
{
	"jsonrpc": "2.0",
	"method": "set_execution_speed",
	"params": {
		"lines_second": 1
	}
}
```

# State modification requests
## **`create_network`**
Frontend -> Backend  
When requested, this should create a new empty network and respond with the new networks GUID  

**Parameters:**  
none  

**Full Example:**  
```json
Request:
{
	"jsonrpc": "2.0",
	"method": "create_network",
	"id": 1
}

Response:
{
	"jsonrpc": "2.0",
	"result": "9af5497a-bd54-4899-ba29-d97b545225a7",
	"id": 1
}
```

## **`create_device`**
Frontend -> Backend  
When requested, this should create a new device with default fields and respond with the new devices GUID  
Device types should follow snake_case naming, for a few examples see the [Mandates](#Mandates)

**Parameters:**  
`type`: The type of device to create  

**Full Example:**  
```json
Request:
{
	"jsonrpc": "2.0",
	"method": "create_device",
	"params": {
		"type": "button"
	},
	"id": 1
}

Response:
{
	"jsonrpc": "2.0",
	"result": "a46cbab4-c27c-4d33-82b0-7c0a1c162356",
	"id": 1
}
```

## **`set_network_value`**
Frontend -> Backend  
When requested, this should set the given field value in the network.  
If `source` is not omitted then the change should originate from that device, though the use of this field is optional.  

**Parameters:**  
`network`: The GUID of the target network  
`field_name`: The field name to change the value of  
`field_value`: The new value for the field  
`source`: The source device GUID of the change (can be omitted)  

**Full Example:**  
```json
Request:
{
	"jsonrpc": "2.0",
	"method": "set_network_value",
	"params": {
		"network": "9af5497a-bd54-4899-ba29-d97b545225a7",
		"field_name": "MyLittleLampOn",
		"field_value": 1
	},
	"id": 1
}

Success Response:
{
	"jsonrpc": "2.0",
	"result": true,
	"id": 1
}
```

## **`set_device_field_name`**
Frontend -> Backend  
When requested, this should set the field name on the device.  

**Parameters:**  
`field_id`: The field ID to change the name of  
`field_name`: The name to change to  

**Full Example:**  
```json
Request:
{
	"jsonrpc": "2.0",
	"method": "set_device_field_name",
	"params": {
		"network": "a46cbab4-c27c-4d33-82b0-7c0a1c162356",
		"field_id": "LampOn",
		"field_name": "MyLittleLampOn"
	},
	"id": 1
}

Success Response:
{
	"jsonrpc": "2.0",
	"result": true,
	"id": 1
}
```
