#ifndef MPU6050_H
#define	MPU6050_H

/**************************************************/
// Constants
#define MPU6050_ADDRESS                 0x68        // I2C Address AD0 = 0
#define MPU6050_ACCEL_LSB               16384       // Accelerometer sensitivity scale factor  LSB/g
#define MPU6050_GYRO_LSB                131         // Gyroscope sensitivity scale factor  LSB/(º/s))
#define MPU6050_ELEMENT_BYTES           4           // Number of bytes of each element in MPU6050_Data
#define MPU6050_MSG_DELIMITER           2.020202f   // Starts and ends a message

/******************************/
// Registers
#define MPU6050_SMPLRT_DIV              0x19        // SMPLRT_DIV[7:0]
#define MPU6050_CONFIG                  0x1A        // -, -, EXT_SYNC_SET[2:0], DLPF_CFG[2:0
#define MPU6050_GYRO_CONFIG             0x1B        // -, -, -, FS_SEL [1:0], -, -, -
#define MPU6050_ACCEL_CONFIG            0x1C        // XA_ST, YA_ST, ZA_ST, AFS_SEL[1:0], -, -, -
#define MPU6050_FIFO_EN                 0x23        // Temp, XG, YG, ZG, ACCEL, SLV2, SLV1, SLV0
#define MPU6050_INT_ENABLE              0x38        // Interruption configurations
#define MPU6050_ACCEL_XOUT_H            0x3B        // ACCEL_XOUT[15:8]
#define MPU6050_ACCEL_XOUT_L            0x3C        // ACCEL_XOUT[7:0]
#define MPU6050_ACCEL_YOUT_H            0x3D        // ACCEL_YOUT[15:8]
#define MPU6050_ACCEL_YOUT_L            0x3E        // ACCEL_YOUT[7:0]
#define MPU6050_ACCEL_ZOUT_H            0x3F        // ACCEL_ZOUT[15:8]
#define MPU6050_ACCEL_ZOUT_L            0x40        // ACCEL_ZOUT[7:0]
#define MPU6050_TEMP_OUT_H              0x41        // TEMP_OUT[15:8]
#define MPU6050_TEMP_OUT_L              0x42        // TEMP_OUT[7:0]
#define MPU6050_GYRO_XOUT_H             0x43        // GYRO_XOUT_H[15:8]
#define MPU6050_GYRO_XOUT_L             0x44        // GYRO_XOUT_L[7:0]
#define MPU6050_GYRO_YOUT_H             0x45        // GYRO_YOUT_H[15:8]
#define MPU6050_GYRO_YOUT_L             0x46        // GYRO_YOUT_L[7:0]
#define MPU6050_GYRO_ZOUT_H             0x47        // GYRO_ZOUT_H[15:8]
#define MPU6050_GYRO_ZOUT_L             0x48        // GYRO_ZOUT_L[7:0]
#define MPU6050_SIGNAL_PATH_RESET       0x68        // -, -, -, -, -, Gyro Rst, Accel Rst, Temp Rst
#define MPU6050_USER_CTRL               0x6A        // -, FIFO_EN, I2C_MST_EN, I2C_IF_DIS, -, FIFO Rst, 12C_MST Rst, SIG_COND Rst
#define MPU6050_PWR_MGMT_1              0x6B        // Reset, Sleep, Cycle, -, Temp dis, ClkSel[2:0]
#define MPU6050_FIFO_COUNT_H            0x72        // FIFO_COUNT[15:8]
#define MPU6050_FIFO_COUNT_L            0x73        // FIFO_COUNT[7:0]
#define MPU6050_FIFO_R_W                0x74        // FIFO_DATA[7:0]
#define MPU6050_WHO_AM_I                0x75        // Upper 6 bits of the MPU6050 address

/**************************************************/
// Structures
union MPU6050_Byteable {
    float value;
    unsigned char bytes[4];
};

typedef struct
{
    union MPU6050_Byteable x, y, z;
} MPU6050_3Axis_Data;

typedef struct
{
    MPU6050_3Axis_Data accelerometer;
    MPU6050_3Axis_Data gyroscope;
    union MPU6050_Byteable temperature;
} MPU6050_Data;

/**************************************************/
// Function declarations
/**
 * \brief Initializes the MPU6050
 *
 * \return Nothing
 */
void MPU6050_Initialize(void);

/**
 * \brief Reads the MPU sensor and saves its values to the data structure
 * 
 * \return Data read from the sensor
 */
MPU6050_Data MPU6050_ReadSensor(void);

/**
 * \brief Resets MPU data structure (all values to 0)
 *
 * \param[in] data Pointer to MPU data structure
 * 
 * \return Nothing
 */
void MPU6050_Reset_Data(MPU6050_Data* data);

/**
 * \brief Converts the data to an array of uint8_t
 *
 * \param[in] data MPU data structure
 * 
 * \return Arraywith data (Start msg, Array size, accelerometer X, accelerometer Y, accelerometer Z, gyroscope X, gyroscope Y, gyroscope Z, temperature, End msg)
 */
uint8_t * MPU6050_Data_2_Uint8(MPU6050_Data data);

#endif	/* MPU6050_H */