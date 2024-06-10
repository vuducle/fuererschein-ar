import os

# Get the current working directory
directory = os.getcwd()

# Loop through all files in the directory
for filename in os.listdir(directory):
    # Check if the filename contains an underscore
    if '_' in filename:
        # Split the filename and get the part after the first underscore
        new_filename = filename.split('_', 1)[-1]
        # Remove any underscores in the remaining part of the filename
        new_filename = new_filename.replace('_', '')
        # Get the full path of the old and new filenames
        old_file = os.path.join(directory, filename)
        new_file = os.path.join(directory, new_filename)
        # Rename the file
        os.rename(old_file, new_file)
        print(f'Renamed: {filename} -> {new_filename}')
