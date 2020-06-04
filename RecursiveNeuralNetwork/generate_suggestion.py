import numpy
from keras.datasets import imdb
from keras.models import Sequential
from keras.layers import Dense
from keras.layers import LSTM
from keras.layers.embeddings import Embedding
from keras.preprocessing import sequence
import xml.etree.ElementTree
import h5py


#Load Model
keras.models.load_model('rnn_weights.h5')