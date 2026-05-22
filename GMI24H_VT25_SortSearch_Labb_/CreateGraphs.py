# In[1]:

import pandas as pd
import os
import matplotlib.pyplot as plt
import seaborn as sns
pd.set_option('display.max_rows', None)


# In[2]:

csv_folder = os.path.join(os.getcwd(),"CSVs/")

all_dfs = []
for entry in os.scandir(csv_folder):
    if entry.is_file() and entry.name.endswith(".csv"):
        df = pd.read_csv(entry.path)
        all_dfs.append((entry.name, df))


# In[3]:

for filename, df in all_dfs:
    plt.figure(figsize=(12,6))
    sns.lineplot(data=df, x="Size", y="MeanTime", label = f"Mean Time, Iterations: {df['Iterations'][0]}")
    mean_plus = df['MeanTime'] + df['StandardDeviation']
    mean_minus = df['MeanTime'] - df['StandardDeviation']
    plt.fill_between(df['Size'], mean_plus, mean_minus, alpha=0.3, label='Standard Deviation')
    plt.title(f"Execution Time — {filename}")
    plt.ylabel("Mean time (microseconds)")
    plt.xlabel("Size")
    plt.legend()
    plt.savefig(f"Graphs/{filename[:-3]}.png")
