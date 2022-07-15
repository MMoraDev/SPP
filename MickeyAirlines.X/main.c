/**************************************************/
// Libraries

/******************************/
// External libraries
#include <stdint.h>
#include <stdio.h>

// MCC Libraries
#include "mcc_generated_files/i2c1_master.h"
#include "mcc_generated_files/mcc.h"
#include "mcc_generated_files/usb/usb.h"

/******************************/
// Internal libraries
#include "mpu6050.h"


/**************************************************/
// Function declarations
void init(void);

void main(void);

/**************************************************/
// Function definitions
void init(void)
{   
   /******************************/
   // USB
   SYSTEM_Initialize();
   
   /******************************/
   // I2C
   I2C1_Initialize();
    
   // Enable the Global Interrupts
   INTERRUPT_GlobalInterruptEnable();

   // Enable the Peripheral Interrupts
   INTERRUPT_PeripheralInterruptEnable();
   
   /******************************/
   // MPU 6050
   MPU6050_Initialize();
}

void main(void)
{
   /******************************/
   // Variables
   MPU6050_Data data;
   
   init();
   
   while (1)
   {
      uint8_t * bytes;
      
      data = MPU6050_ReadSensor();
      
      bytes = MPU6050_Data_2_Uint8(data);
      
      if(USBUSARTIsTxTrfReady() == true) putUSBUSART(bytes, (uint8_t)*(float *)&bytes[MPU6050_ELEMENT_BYTES * 1]);
      
      CDCTxService();
      __delay_ms(2);
   }
}