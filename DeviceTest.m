clear all; close all hidden; clc;

format long;

mData = zeros(1, 0) % Contains the information from the sensor

instrreset;

maxPoints = 30;            
device = serial('COM3');           
fopen(device);

for i = 1:1:100
    mTemp = fread(device, 10, 'float32');
    mData(end + 1) = mTemp(3);
    
    if length(mData) > maxPoints
        mData = mData(end - maxPoints : end);
    end
    
    plot(mData);
    drawnow;
end

fclose(device);
