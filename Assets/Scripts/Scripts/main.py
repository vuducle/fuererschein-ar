import os
import json
import unicodedata

# Function to extract description from filename
def extract_description(filename):
    # Split filename by underscore
    parts = filename.split('_')
    # Remove file extension
    description = parts[-1].split('.')[0]
    # Join remaining parts with space
    if len(parts) > 2:
        description = ' '.join(parts[2:-1])
    elif description.isdigit():
        description = parts[-2] # if description is a number, take the preceding part
    # Normalize description to handle special characters
    description = unicodedata.normalize('NFC', description)
    # Handle special cases
    if description in ["", "Carsharing"]:
        # If description is empty or "Carsharing", use the first part of the filename
        description = parts[0]
    return description

# Directory containing images (current directory)
directory = os.getcwd()

# Initialize dictionary to store mappings
image_descriptions = {}

# Iterate through files in directory
for filename in os.listdir(directory):
    if filename.endswith(".png"): # Assuming all images are PNG files
        # Extract description from filename
        description = extract_description(filename)
        # Map filename to description
        image_descriptions[filename] = description

# Path to save JSON file
json_file_path = os.path.join(directory, "image_descriptions.json")

# Save mappings to JSON file
with open(json_file_path, "w", encoding='utf-8') as json_file:
    json.dump(image_descriptions, json_file, ensure_ascii=False, indent=4)

print("JSON file created successfully.")
