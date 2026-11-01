# Hosts File Recovery Tool

A lightweight Windows utility to **detect, back up, and restore** the system `hosts` file — with a single click.

---

## Features

- **Auto-detection** — checks whether `C:\Windows\System32\drivers\etc\hosts` exists at startup
- **Live preview** — displays the current hosts file content in a read-only editor
- **Safe backup** — saves timestamped `.bak` snapshots to an Administrators/SYSTEM-only `%ProgramData%` folder
- **Auto-snapshot before every restore** — the current hosts content is saved automatically right before "Restore Default" or "Restore from Backup" overwrites it, so a misclick is never unrecoverable
- **Dual restore modes**
  - Restore to Windows factory default
  - Restore from any previously saved backup
- **Atomic write** — restores are written to a temp file and swapped into place, so a crash/power loss mid-write can't leave the hosts file truncated
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
git clone https://github.com/ThanhCode93x/Hosts-File-Recovery-Tool.git
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

Backups are saved to:

```
%ProgramData%\HostsFileRecoveryTool\Backups\
```

named `hosts_YYYYMMDD_HHmmss_fff.bak` (manual) or `autosave_before_restore_YYYYMMDD_HHmmss_fff.bak` (automatic, taken right before a restore overwrites the file). This folder is created with an ACL restricted to Administrators/SYSTEM — see [Security Notes](#security-notes).

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
└── app.manifest              # UAC manifest (requireAdministrator)
```

> Backups live outside this folder, under `%ProgramData%\HostsFileRecoveryTool\Backups\` — see [Usage](#usage).

---

## Security Notes

- The `hosts` file is re-locked as **Read-Only** after every write to reduce the attack surface for malware that modifies it
- Backups live under `%ProgramData%\HostsFileRecoveryTool\Backups\` with an ACL restricted to **Administrators/SYSTEM only**. This matters because the tool runs elevated and restores a selected `.bak` file by writing its content straight into the system `hosts` file: if the backup folder were world-writable (e.g. sitting next to the `.exe` on the Desktop/Downloads), a non-elevated process could plant a malicious `.bak` file there and have it silently written to the system `hosts` file the next time an admin clicks "Restore from Backup"
- Restores are written atomically (temp file + swap) so a crash or power loss mid-write can't leave the `hosts` file truncated
- The tool itself requires Administrator rights — never run untrusted executables with elevated privileges
- Always verify the source when downloading pre-built binaries

---

## License

This project is licensed under the **MIT License** — see [LICENSE](LICENSE) for details.
