import rospy
from std_msgs.msg import Float32
from sensor_msgs.msg import Imu
import csv
from bisect import bisect_left
import sys  # Import sys module to manipulate Python path
from datetime import datetime

# Add the path to your Catkin workspace to Python path
sys.path.append('../../catkin_ws/devel/lib/python2.7/dist-packages')

# Import your custom message
from racepak.msg import rp_wheel_encoders  # Assuming racepak is the package name

class WheelEncodersPublisher:
    def __init__(self):
        rospy.init_node('rosbag_timestamp_reader', anonymous=True)
        
        # Create publisher for custom message
        self.pub = rospy.Publisher('/wheel_rpm_republished', rp_wheel_encoders, queue_size=10)
        
        # Load wheel rpm data from the CSV file
        self.rpm_data = self.load_csv_data()
        
        # Subscribe to necessary topics
        rospy.Subscriber('/multisense/imu/imu_data', Imu, self.callback)
        
        rospy.spin()

    def load_csv_data(self):
        rpm_data = []
        with open('csv0910_wheel_rpm/20210910_30-wheel_rpm.csv', 'r') as csv_file:
            csv_reader = csv.DictReader(csv_file, delimiter=',')
            for row in csv_reader:
                # Parse the timestamp from the first column
                timestamp_str = row['time']
                timestamp = datetime.strptime(timestamp_str, "%Y/%m/%d/%H:%M:%S.%f")
                # Convert the timestamp to seconds
                timestamp_sec = rospy.Time.from_sec(timestamp.timestamp())
                # Append the timestamp and wheel rpm values to the rpm_data list
                rpm_data.append((timestamp_sec, float(row['.front_left']), float(row['.front_right']), float(row['.rear_left']), float(row['.rear_right'])))
        return rpm_data

    def find_closest_values(self, timestamp):
        # Find index of the closest timestamp in the rpm_data list
        index = bisect_left(self.rpm_data, (timestamp,))
        
        # Handle edge cases where index is at the beginning or end of the list
        if index == 0:
            return self.rpm_data[0][1:]  # Return the first row
        if index == len(self.rpm_data):
            return self.rpm_data[-1][1:]  # Return the last row
        
        # Return values from the row with the closest timestamp
        return self.rpm_data[index][1:]

    def callback(self, data):
        # Output the timestamp of the currently received message
        timestamp = rospy.Time.from_sec(data.header.stamp.to_sec())

        # Find closest values from rpm_data for the given timestamp
        front_left, front_right, rear_left, rear_right = self.find_closest_values(timestamp)

        # Publish custom message
        msg = rp_wheel_encoders()
        msg.front_left = front_left
        msg.front_right = front_right
        msg.rear_left = rear_left
        msg.rear_right = rear_right
        self.pub.publish(msg)

if __name__ == '__main__':
    print("Republishing...")
    WheelEncodersPublisher()
