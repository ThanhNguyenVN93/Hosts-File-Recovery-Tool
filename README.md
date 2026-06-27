# Hosts File Recovery Tool

A lightweight Windows utility to **detect, back up, and restore** the system `hosts` file — with a single click.

---

## Features

- **Auto-detection** — checks whether `C:\Windows\System32\drivers\etc\hosts` exists at startup
- **Live preview** — displays the current hosts file content in a read-only editor
- **Safe backup** — saves timestamped `.bak` snapshots to a local `Backups/` folder
- **Dual restore modes**
  - Restore to Windows factory default
  - Restore from any previously saved backup
- **Read-Only protection** — automatically strips the `ReadOnly` attribute before writing, then re-applies it afterward to guard against malware
- **DNS flush** — runs `ipconfig /flushdns` silently after every restore so changes take effect immediately
- **UAC elevation** — requests Administrator privileges at launch; exits gracefully if denied
- **Input validation** — rejects empty or whitespace-only backup files before they can corrupt the system file
- **BOM-free UTF-8** — writes without Byte Order Mark to ensure full compatibility with the Windows DNS resolver

---

## Requirements

| Requirement | Minimum |
|---|---|
| OS | Windows 7 SP1 (build 7601) or later |
| Runtime | .NET Framework 4.5.2 |
| Privileges | Administrator |

> **Note:** Windows 7 RTM (build 7600) and older are explicitly blocked at startup.

---

## Getting Started

### Option A — Download release

1. Go to the [Releases](../../releases) page
2. Download `Hosts.File.Recovery.Tool.exe`
3. Right-click → **Run as administrator**

### Option B — Build from source

```
git clone https://github.com/ThanhCode93x/frm_hosts_file_recovery.git
```

Open `frm_hosts_file_recovery.slnx` in **Visual Studio 2022** (or later) and press **Ctrl + Shift + B**.

---

## Usage

| Action | How |
|---|---|
| View hosts file | Displayed automatically on startup |
| Back up current file | Click **Back Up** |
| Restore Windows default | Click **Restore Default** → confirm |
| Restore a saved backup | Select a `.bak` entry from the dropdown → click **Restore from Backup** → confirm |
| Refresh status | Click **Refresh** |

Backups are saved to a `Backups/` folder next to the executable, named:

```
hosts_YYYYMMDD_HHmmss.bak
```

---

## Project Structure

```
frm_hosts_file_recovery/
├── Form1.cs                  # Main form — all application logic
├── Form1.Designer.cs         # Auto-generated UI layout
├── Program.cs                # Entry point — UAC + OS version guard
├── Properties/
│   └── AssemblyInfo.cs       # Assembly metadata
├── app.ico                   # Application icon (48/32/16 px)
├── app.manifest              # UAC manifest (requireAdministrator)
└── Backups/                  # Created at runtime — stores .bak files
```

---

## Security Notes

- The `hosts` file is re-locked as **Read-Only** after every write to reduce the attack surface for malware that modifies it
- The tool itself requires Administrator rights — never run untrusted executables with elevated privileges
- Always verify the source when downloading pre-built binaries

---

## License

This project is licensed under the **MIT License** — see [LICENSE](LICENSE) for details.
