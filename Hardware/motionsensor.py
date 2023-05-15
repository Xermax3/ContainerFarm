from grove.gpio import GPIO

class GroveMiniPIRMotionSensor(GPIO):
  def __init__(self, pin):
    super(GroveMiniPIRMotionSensor, self).__init__(pin, GPIO.IN)
    self._on_detect = None

  @property
  def on_detect(self):
    return self._on_detect

  @on_detect.setter
  def on_detect(self, callback):
    if not callable(callback):
      return

    if self.on_event is None:
      self.on_event = self._handle_event

    self._on_detect = callback

  def _handle_event(self, pin, value):
    if value:
      if callable(self._on_detect):
        self._on_detect()