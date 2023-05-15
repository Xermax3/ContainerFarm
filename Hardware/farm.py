import argparse
import os
import sys
import time
import traceback
from dataclasses import dataclass
import datetime
from typing import List

from azure.iot.device import IoTHubDeviceClient, Message, MethodRequest, MethodResponse
from dotenv import load_dotenv

from security import Security
from location import Location
from plant import Plant

TELEMETRY_INTERVAL = 10

@dataclass
class Arguments:
  all: bool
  angles: bool
  buzzer: List[str]
  door: bool
  dummy: bool
  fan: List[str]
  gps: bool
  light: List[str]
  lock: List[str]
  luminosity: bool
  moisture: bool
  motion: bool
  noise: bool
  temp_humi: bool
  verbose: bool
  vibration: bool
  water: bool

  @staticmethod
  def from_args():
    parser = argparse.ArgumentParser()

    parser.add_argument(
      '--all',
      action='store_true',
      help='Upload all telemetry to azure IoT hub'
    )

    parser.add_argument(
      '--angles',
      '-a',
      action='store_true',
      help='Upload all geo-location angle related telemetry to azure IoT hub'
    )

    parser.add_argument(
      '--buzzer',
      '-b',
      nargs='*',
      help='Read or control (on/off) the reTerminals built-in buzzer'
    )

    parser.add_argument(
      '--door',
      '-d',
      action='store_true',
      help='Upload all security door related telemetry to azure IoT hub'
    )

    parser.add_argument(
      '--dummy',
      action='store_true',
      help='Send random data for every desired subsystem (prevents relying on hardware)'
    )

    parser.add_argument(
      '--fan', '-f', nargs='*', help='Read or control (on/off) plant fan state'
    )

    parser.add_argument(
      '--gps',
      '-g',
      action='store_true',
      help='Upload all geo-location gps related telemetry to azure IoT hub'
    )

    parser.add_argument(
      '--light', nargs='*', help='Read or control (on/off) plant light state'
    )

    parser.add_argument(
      '--lock',
      '-l',
      nargs='*',
      help='Upload all security lock related telemetry to azure IoT hub'
    )

    parser.add_argument(
      '--luminosity',
      action='store_true',
      help='Upload all security luminosity related telemetry to azure IoT hub'
    )

    parser.add_argument(
      '--moisture',
      action='store_true',
      help='Upload all plant soil moisture related telemetry to azure IoT hub'
    )

    parser.add_argument(
      '--motion',
      action='store_true',
      help='Upload all security motion related telemetry to azure IoT hub'
    )

    parser.add_argument(
      '--noise',
      '-n',
      action='store_true',
      help='Upload all security noise related telemetry to azure IoT hub'
    )

    parser.add_argument(
      '--temp_humi',
      '-t',
      action='store_true',
      help=
      'Upload all plant temperature and humidity related telemetry to azure IoT hub'
    )

    parser.add_argument(
      '--verbose',
      required=False,
      action='store_true',
      help='Add tracebacks to error messages'
    )

    parser.add_argument(
      '--vibration',
      '-v',
      action='store_true',
      help='Upload all geo-location vibration related telemetry to azure IoT hub'
    )

    parser.add_argument(
      '--water',
      '-w',
      action='store_true',
      help='Upload all plant water related telemetry to azure IoT hub'
    )

    return Arguments(**vars(parser.parse_args()))

  def run(self):
    ContainerFarm(self).run()



@dataclass
class Config:
  device_id: str
  iothub_conn_str: str
  iothub_device_conn_str: str

  @staticmethod
  def from_env():
    load_dotenv()
    return Config(
      device_id=os.getenv("DEVICE_ID"),
      iothub_conn_str=os.getenv("IOTHUB_CONNECTION_STRING"),
      iothub_device_conn_str=os.getenv("IOTHUB_DEVICE_CONNECTION_STRING"),
    )

class Client:
  def __init__(self):
    self.config = Config.from_env()
    self.device = self.__device()
    self.message = []

  def __del__(self):
    self.device.shutdown()

  def add(self, subsystem, data):
    if data is None:
      return

    for key, value in data.items():
      print(f'Adding data for {key}: {value}')
      self.message.append({
        'SubSystem': subsystem,
        'Field': str(key),
        'Value': str(value),
        'EntryDate': datetime.datetime.now().strftime("%m/%d/%Y, %H:%M:%S")
      })

  def send(self):
    message = Message(str(self.message))
    print(f'[~] Sending data to IoT hub...')
    self.device.send_message(message)
    self.message = []
    time.sleep(TELEMETRY_INTERVAL)

  def __device(self):
    conn_str = self.config.iothub_device_conn_str
    device = IoTHubDeviceClient.create_from_connection_string(conn_str)
    device.on_twin_desired_properties_patch_received = self.__patch_handler
    device.connect()
    return device

  def __patch_handler(self, patch):
    global TELEMETRY_INTERVAL
    print('Twin patch received')
    interval = patch.get('telemetryInterval')
    if interval:
      TELEMETRY_INTERVAL = interval
      print(f'Updated telemetry interval: {interval}')
      self.device.patch_twin_reported_properties(patch.get('telemetryInterval'))

class ContainerFarm:
  def __init__(self, args):
    self.args = args
    self.client = Client()
    self.client.device.on_method_request_received = self.__method_handler
    self.subsystems = [Security(), Location(), Plant()]
    # If no sensor is specified, send all
    for prop, value in vars(self.args).items():
      if prop not in ('dummy', 'verbose') and value not in (False, None):
        return
    self.args.all = True

  def run(self):
    while True:
      # Go through each argument property name and value
      for prop, value in vars(self.args).items():
        # Go through each subsystem
        for subsystem in self.subsystems:
          # Ensure the subsystem has the method name that matches the argument, and the argument
          # contains a value we care about, or all is passed
          if (value not in [False, None] or self.args.all) and hasattr(subsystem, prop):
            # Convert the value to a list
            args = ([value], value)[isinstance(value, list)]
            # Indicate whether or not to send dummy data
            args.append(self.args.dummy)
            # Add the data to the list
            self.client.add(
              subsystem=str(subsystem), data=getattr(subsystem, prop)(*args)
            )
      # Send data received from the subsystem method to iot hub
      self.client.send()

  def __method_handler(self, method: MethodRequest):
    methods = ['lock', 'buzzer', 'fan', 'light']

    for subsystem in self.subsystems:
      if method.name in methods and hasattr(subsystem, method.name):
        getattr(subsystem, method.name)(method.payload['state'])
        status, payload = 200, {'Response': f'Executed direct method {method.name}'}
      else:
        status, payload = 404, {'Response': f'Direct method {method.name} not defined'}

    self.client.device.send_method_response(
      MethodResponse.create_from_method_request(method, status, payload)
    )

def err(*args, **kwargs):
  print(*args, **kwargs, file=sys.stderr)
  sys.exit(1)

def main(args):
  try:
    args.run()
  except Exception as error:
    if args.verbose:
      traceback.print_exc()
    err(f'error: {error}')

if __name__ == '__main__':
  main(Arguments.from_args())
