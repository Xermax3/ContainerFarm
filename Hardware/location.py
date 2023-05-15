try:
  from seeed_python_reterminal.acceleration import AccelerationEvent, AccelerationName
  import seeed_python_reterminal.core as rt
  import pynmea2
  from serial import Serial
except:
  AccelerationEvent = None
  AccelerationName = None
  rt = None
  pynmea2 = None
  Serial = None
from math import atan, pi, sqrt
import random

class Location:
  def __init__(self, args=None):
    self.args = args
    self.dummy_buzzer = False

  def __str__(self):
    return 'location'

  def angles(self, *args):
    if args[1] == True:
      return {'pitch': random.uniform(0,360), 'roll': random.uniform(0,360)}

    points = {}
    for event in self.__read_accelerometer():
      points[event.name] = event.value
      if len(points) == 3:
        return {
          'pitch':
          180 * atan(
            points[AccelerationName.X] / sqrt(
              pow(points[AccelerationName.Y], 2) +
              pow(points[AccelerationName.Z], 2)
            )
          ) / pi,
          'roll':
          180 * atan(
            points[AccelerationName.Y] / sqrt(
              pow(points[AccelerationName.X], 2) +
              pow(points[AccelerationName.Z], 2)
            )
          ) / pi,
        }

  def buzzer(self, *args): 
    if args[1] == True:
      if args[0] != None:
        self.dummy_buzzer = ('on', 'off')[args[0].lower()]
      return {'buzzer': ('on', 'off')[self.dummy_buzzer]}
    
    if args[0] != None:
      try:
        rt.buzzer = args[0].lower()
      except KeyError:
        raise ValueError(f'Invalid buzzer argument: {args[0]}')
    return {'buzzer': ('on', 'off')[rt.buzzer]}

  def gps(self, *args):
    if args[1] == True:
      return {'latitude': random.uniform(-90,90), 'longitude': random.uniform(-90,90)}
    
    serial = Serial('/dev/ttyAMA0', 9600, timeout=1)
    serial.reset_input_buffer()
    serial.flush()
    try:
        line = serial.readline().decode('utf-8')
        while len(line) > 0:
          data = pynmea2.parse(line.rstrip())
          return {'latitude': data.lat, 'longitude': data.lon}
    except:
        line = serial.readline().decode('utf-8')


  def vibration(self, *args):
    if args[1] == True:
      return {'vibration': random.uniform(0,500)}

    prev, vibration, accuracy = {}, 0, 10
    for index, event in enumerate(self.__read_accelerometer()):
      if event.name not in prev:
        prev[event.name] = event.value
      else:
        vibration += event.value - prev[event.name]
        prev[event.name] = event.value
      if index == accuracy:
        return {'vibration': vibration}

  def __read_accelerometer(self):
    device = rt.get_acceleration_device()
    while True:
      for event in device.read_loop():
        accel_event = AccelerationEvent(event)
        if accel_event.name:
          yield accel_event