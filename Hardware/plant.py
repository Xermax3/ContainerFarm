try:
  from grove import grove_temperature_humidity_aht20
  from grove.adc import ADC
  from gpiozero import DigitalOutputDevice
  import chainable_rgb_direct
except:
  grove_temperature_humidity_aht20 = None
  ADC = None
  DigitalOutputDevice = None
  chainable_rgb_direct = None
import random

class Plant:
  def __init__(self):
    if grove_temperature_humidity_aht20 != None and DigitalOutputDevice != None and ADC != None and chainable_rgb_direct != None:
      self.i2c = grove_temperature_humidity_aht20.GroveTemperatureHumidityAHT20(
        bus=4
      )
      self.fan_device = DigitalOutputDevice(18)
      self.adc = ADC()
      self.light_device = chainable_rgb_direct.rgb_led(2)
    self.dummy_fan = False
    self.dummy_light = False

  def __str__(self):
    return 'plant'

  def temp_humi(self, *args):
    if args[1] == True:
      return {'temperature': random.uniform(10,30), 'humidity': random.uniform(0,200)}
    
    temp, humi = self.i2c.read()
    return {'temperature': temp, 'humidity': humi}

  def water(self, *args):
    if args[1] == True:
      return {'water': random.uniform(0,1000)}
    
    return {'water': self.adc.read_voltage(4)}

  def moisture(self, *args):
    if args[1] == True:
      return {'moisture': random.uniform(0,200)}
    
    return {'moisture': self.adc.read_voltage(2)}

  def fan(self, *args):
    if args[1] == True:
      if args[0] != None:
        self.dummy_fan = ('off', 'on')[args[0].lower()]
      return {'fan': ('off', 'on')[self.dummy_fan]}
    
    if args[0] != None:
      new_state = args[0].lower()
      if new_state == 'on':
        self.fan_device.on()
      elif new_state == 'off':
        self.fan_device.off()
      else:
        raise ValueError(
          f"Invalid fan state {new_state}. Must be 'on' or 'off'"
        )
    return {'fan': ('off', 'on')[self.fan_device.is_active]}

  def light(self, *args):
    if args[1] == True:
      if args[0] != None:
        self.dummy_light = ('off', 'on')[args[0].lower()]
      return {'light': ('off', 'on')[self.dummy_light]}
    
    if args[0] != None:
      new_state = args[0].lower()
      if new_state == 'on':
        self.light_device.setOneLED(255, 255, 255, 0)
        self.light_device.setOneLED(255, 255, 255, 1)
      elif new_state == 'off':
        self.light_device.setOneLED(0, 0, 0, 0)
        self.light_device.setOneLED(0, 0, 0, 1)
      else:
        raise ValueError(
          f"Invalid light state {new_state}. Must be 'on' or 'off'"
        )
    return {
      'light':
      ('off', 'on')[0 not in (
        self.light_device.r_all[0],
        self.light_device.r_all[1],
        self.light_device.g_all[0],
        self.light_device.g_all[1],
        self.light_device.b_all[0],
        self.light_device.b_all[1]
      )]
    }
