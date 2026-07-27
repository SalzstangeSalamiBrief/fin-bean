# Workflow

## Agents

There are these agents that should be used:

- planner.agent.md
  - used for orchestrating the project
  - breaks the project/product.md spec down into individual feature/bug files
  - assigns feature/bug numbers and keeps the status (todo/active/finished) up to date
  - splits features into smaller parts to increase review quality
- expert-react-frontend-engineer.agent.md
  - used for Frontend development
  - Writes the frontend code
- c#-expert.agent.md
  - used for backend development
  - writes the backend code
- qa.agent.md
  - used for planning qa processes
  - Writes Tests FE/BE
  - Bugs will be added as md file to the bug folder

Each agent will be used for their corresponding tasks. Before starting any task, an agent must read `project/product.md`, `project/architecture.md` and this file (`project/workflow.md`) plus the relevant feature/bug file in `project/features` or `project/bugs` to stay aligned with the current spec and workflow.

## Spec lifecycle

- the project/product.md file contains the whole spec of the project and will change over time
  - the user/product owner authors and maintains this file; the planner agent only reads it and breaks it down into feature/bug files, it does not change the spec itself
- Each feature in the project/product.md file will added to its own md file in the project/features
  - the content will be concise
- after each feature/bug is defined the user will review these files
  - features will be split into smaller parts to increase review quality
- If a feature/bug is getting built/fixed then a new git branch `<feature|bugfix>/<feature_name>` will be created

### Branching strategy

- `main`: the stable, released version
- `develop`: the version currently under development
- feature branches are prefixed `feature/`, bug fix branches are prefixed `bugfix/`
- every `feature/` or `bugfix/` branch is created from `develop` and merged back into `develop`
- every `feature/` or `bugfix/` branch is compared/diffed against `develop` (not `main`)
- on approval, the branch is merged into `develop` using a squash commit, so each feature/bugfix results in exactly one commit on `develop`
- merging `develop` into `main` is done manually by the user; agents do not merge into `main`

### Feature files

- File name: `feature_<number>_<short_description>.<status>.md`
  - status
    - If the feature is planned but not started, the file name contains `_todo_`
    - If the feature is currently under development, the file name contains `_active_`
    - If the feature is implemented, the file name contains `_finished_`
  - the number starts at 1 and increments with each new file
- Structure:
  ```md
      # <feature_name>
      ## Description              <!-- Description of the feature -->
      ## Acceptance criteria      <!-- Acceptance Criteria -->
      ## implementation           <!-- After finishing the implementation a short summary of the implementation details is given -->
  ```

### Bug files

- File name: `bug_<number>_<short_description>.<status>.md`
  - status
    - If the bug is planned but not started, the file name contains `_todo_`
    - If the bug is currently under development, the file name contains `_active_`
    - If the bug is implemented, the file name contains `_finished_`
  - the number starts at 1 and increments with each new file

- Structure:
  ```md
      # <bug_name>
      ## Description              <!-- Description of the feature -->
      ## How to reproduce         <!-- step by step instructions to reproduce the bug-->
      ## Acceptance criteria      <!-- Acceptance Criteria -->
      ## implementation           <!-- After finishing the implementation a short summary of the implementation details is given -->
  ```
