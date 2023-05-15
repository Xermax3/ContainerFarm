try:
  from gpiozero import Servo
  from grove.adc import ADC
  import seeed_python_reterminal.core as rt
  import gpiozero
  from motionsensor import GroveMiniPIRMotionSensor
except:
  Servo = None
  ADC = None
  rt = None
  gpiozero = None
  GroveMiniPIRMotionSensor = None
import math
import random

class Security:
  def __init__(self):
    if Servo != None and ADC != None and gpiozero != None:
      self.__servo = Servo(12)
      self.__door = gpiozero.InputDevice(5)
      self.__sensor = ADC()
      self.motion_detect = False
      self.motion_detected = False
      self.motion_sensor = None
    self.dummy_buzzer = False
    self.dummy_lock = False

  def __str__(self):
    return 'security'

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

  def door(self, *args):
    if args[1] == True:
      return {'door': random.choice(['closed', 'open'])}

    return {'door': ('closed', 'open')[self.__door.is_active]}

  def lock(self, *args):
    if args[1] == True:
      if args[0] != None:
        self.dummy_lock = ('closed', 'open')[args[0].lower()]
      return {'lock': ('closed', 'open')[self.dummy_lock]}
    
    if args[0] != None:
      if args[0].lower() == 'closed':
        self.__servo.min()
      if args[0].lower() == 'open':
        self.__servo.max()
    return {'lock': ('closed', 'open')[self.__servo.value == 1.0]}

  def luminosity(self, *args):
    if args[1] == True:
      return {'luminosity': random.uniform(0,30000)}

    return {'luminosity': str(rt.illuminance)}

  def noise(self, *args):
    if args[1] == True:
      return {'noise': random.uniform(0,600)}
    
    return {'noise': str(math.log(20, self.__sensor.read_voltage(0)) * 100)}

  def motion(self, *args):
    if args[1] == True:
      return {'motion': random.choice(['detected', 'none'])}
    
    if not self.motion_detect:
      self.motion_sensor = GroveMiniPIRMotionSensor(16)

      def callback():
        self.motion_detected = True

      self.motion_sensor.on_detect = callback
      self.motion_detect = True

    return {'motion': ('detected', 'none')[self.motion_detected]}